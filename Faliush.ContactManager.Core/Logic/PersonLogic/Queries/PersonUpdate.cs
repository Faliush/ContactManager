using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Core.Services;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonUpdateRequest(PersonUpdateViewModel Model, ClaimsPrincipal User) 
    : IRequest<OperationResult<PersonViewModel>>;

public class PersonUpdateRequestHandler : IRequestHandler<PersonUpdateRequest, OperationResult<PersonViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateCalcualtorService _dateCalcualtorService;
    public PersonUpdateRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, 
        IDateCalcualtorService dateCalcualtorService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateCalcualtorService = dateCalcualtorService;
    }

    public async Task<OperationResult<PersonViewModel>> Handle(PersonUpdateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<PersonViewModel>();
        var repository = _unitOfWork.GetRepository<Person>();

        var item = await repository
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Email == request.Model.Email && x.Id != request.Model.Id
            );

        if (item is not null)
        {
            operation.AddError(new ContactManagerArgumentException($"person with email {request.Model.Email} already exist"));
            return operation;
        }

        var entity = await repository
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Model.Id,
                disableTracking: false
            );

        if (entity is null)
        {
            operation.AddError(new ContactManagerNotFoundException($"person with id: {request.Model.Id} not found"));
            return operation;
        }

        var country = await _unitOfWork.GetRepository<Country>()
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Model.CountryId
            );

        if (country is null)
        {
            operation.AddError(new ContactManagerNotFoundException($"country with id {request.Model.CountryId} doesn't exist"));
            return operation;
        }

        _mapper.Map(request.Model, entity, x => x.Items[nameof(IdentityUser)] = request.User.Identity!.Name);

        repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
        {
            var exception = _unitOfWork.LastSaveChangeResult.Exception ?? new ContactManagerSaveDatabaseException();
            operation.AddError(exception);
            return operation;
        }

        var result = _mapper.Map<PersonViewModel>(entity);
        result.Age = _dateCalcualtorService.GetTotalYears(entity.DateOfBirth);
        result.CountryName = country.Name;

        operation.Result = result;
        return operation;
    }
}
