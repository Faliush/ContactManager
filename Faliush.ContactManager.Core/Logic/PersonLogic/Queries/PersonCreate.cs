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

public record PersonCreateRequest(PersonCreateViewModel Model, ClaimsPrincipal User) : IRequest<OperationResult<PersonViewModel>>;

public class PersonCreateRequestHandler : IRequestHandler<PersonCreateRequest, OperationResult<PersonViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateCalcualtorService _dateCalcualtorService;

    public PersonCreateRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, 
        IDateCalcualtorService dateCalcualtorService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateCalcualtorService = dateCalcualtorService;
    }

    public async Task<OperationResult<PersonViewModel>> Handle(PersonCreateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<PersonViewModel>();
        var repository = _unitOfWork.GetRepository<Person>();

        var item = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Email == request.Model.Email
            );

        if (item is not null)
        {
            operation.AddError(new ContactManagerArgumentException($"person with email {request.Model.Email} already exist"));
            return operation;
        }

        var entity = _mapper.Map<Person>(request.Model, x => x.Items[nameof(IdentityUser)] = request.User.Identity!.Name);

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

        await repository.InsertAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
        {
            var exception = _unitOfWork.LastSaveChangeResult.Exception ?? new ContactManagerSaveDatabaseException();
            operation.AddError(exception);
            return operation;
        }

        var result = _mapper.Map<PersonViewModel>(entity);
        result.Age = _dateCalcualtorService.GetTotalYears(result.DateOfBirth);
        result.CountryName = country.Name;

        operation.Result = result;

        return operation;

    }
}
