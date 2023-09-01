using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Core.Services;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using Faliush.ContactManager.Models.Base;
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
    private readonly IStringConvertService _stringConvertService;
    private readonly IDateCalcualtorService _dateCalcualtorService;
    public PersonUpdateRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, 
        IStringConvertService stringConvertService, 
        IDateCalcualtorService dateCalcualtorService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _stringConvertService = stringConvertService;
        _dateCalcualtorService = dateCalcualtorService;
    }

    public async Task<OperationResult<PersonViewModel>> Handle(PersonUpdateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<PersonViewModel>();
        var repository = _unitOfWork.GetRepository<Person>();

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
        entity.Gender = _stringConvertService.ConvertToEnum<GenderOptions>(request.Model.Gender);

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
