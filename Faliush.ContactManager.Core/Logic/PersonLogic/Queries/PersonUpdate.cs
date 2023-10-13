using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Core.Services;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonUpdateRequest(PersonUpdateViewModel Model, ClaimsPrincipal User) 
    : IRequest<OperationResult<PersonViewModel>>;

public class PersonUpdateRequestHandler : IRequestHandler<PersonUpdateRequest, OperationResult<PersonViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateCalcualtorService _dateCalcualtorService;
    private readonly ILogger<PersonUpdateRequestHandler> _logger;
    public PersonUpdateRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, 
        IDateCalcualtorService dateCalcualtorService,
        ILogger<PersonUpdateRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateCalcualtorService = dateCalcualtorService;
        _logger = logger;
    }

    public async Task<OperationResult<PersonViewModel>> Handle(PersonUpdateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<PersonViewModel>();
        var repository = _unitOfWork.GetRepository<Person>();

        _logger.LogInformation("PersonUpdateRequestHandler checks given email for existance in database");
        var item = await repository
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Email == request.Model.Email && x.Id != request.Model.Id
            );

        if (item is not null)
        {
            _logger.LogError("Given email already exist in database");
            operation.AddError(new ContactManagerArgumentException($"person with email {request.Model.Email} already exist"));
            return operation;
        }

        _logger.LogInformation("PersonUpdateRequestHandler checks given person id for existance in database");
        var entity = await repository
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Model.Id,
                disableTracking: false
            );

        if (entity is null)
        {
            _logger.LogError("Given person id doesn't exist in database");
            operation.AddError(new ContactManagerNotFoundException($"person with id: {request.Model.Id} not found"));
            return operation;
        }

        _logger.LogInformation("PersonUpdateRequestHandler checks given country id for existance in database");
        var country = await _unitOfWork.GetRepository<Country>()
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Model.CountryId
            );

        if (country is null)
        {
            _logger.LogError("Given country id doesn't exist in database");
            operation.AddError(new ContactManagerNotFoundException($"country with id {request.Model.CountryId} doesn't exist"));
            return operation;
        }

        _mapper.Map(request.Model, entity, x => x.Items[nameof(IdentityUser)] = request.User.Identity!.Name);

        repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Update person");

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
        {
            _logger.LogError("Arose exception during data saving");
            var exception = _unitOfWork.LastSaveChangeResult.Exception ?? new ContactManagerSaveDatabaseException();
            operation.AddError(exception);
            return operation;
        }

        var result = _mapper.Map<PersonViewModel>(entity);
        result.Age = _dateCalcualtorService.GetTotalYears(entity.DateOfBirth);
        result.CountryName = country.Name;

        operation.Result = result;
        _logger.LogInformation("Person was updated successfully");
        return operation;
    }
}
