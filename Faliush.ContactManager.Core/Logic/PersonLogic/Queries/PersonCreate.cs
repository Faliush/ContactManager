using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Core.Services.Interfaces;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonCreateRequest(PersonCreateViewModel Model, ClaimsPrincipal User) : IRequest<OperationResult<PersonViewModel>>;

public class PersonCreateRequestHandler : IRequestHandler<PersonCreateRequest, OperationResult<PersonViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateCalcualtorService _dateCalcualtorService;
    private readonly ILogger<PersonCreateRequestHandler> _logger;   

    public PersonCreateRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, 
        IDateCalcualtorService dateCalcualtorService,
        ILogger<PersonCreateRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateCalcualtorService = dateCalcualtorService;
        _logger = logger;
    }

    public async Task<OperationResult<PersonViewModel>> Handle(PersonCreateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<PersonViewModel>();
        var repository = _unitOfWork.GetRepository<Person>();

        _logger.LogInformation("PersonCreateRequestHandler checks email for existance in database");
        var item = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Email == request.Model.Email
            );

        if (item is not null)
        {
            _logger.LogError("Given email already exist in database");
            operation.AddError(new ContactManagerArgumentException($"person with email {request.Model.Email} already exist"));
            return operation;
        }

        var entity = _mapper.Map<Person>(request.Model, x => x.Items[nameof(IdentityUser)] = request.User.Identity!.Name);

        _logger.LogInformation("PersonCreateRequestHandler find given country id in database");
        var country = await _unitOfWork.GetRepository<Country>()
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Model.CountryId
            );

        if (country is null)
        {
            _logger.LogInformation("Given country id doesn't exist in database");
            operation.AddError(new ContactManagerNotFoundException($"country with id {request.Model.CountryId} doesn't exist"));
            return operation;
        }

        await repository.InsertAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Add given person to the database");

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
        {
            _logger.LogError("Arose exception during data saving");
            var exception = _unitOfWork.LastSaveChangeResult.Exception ?? new ContactManagerSaveDatabaseException();
            operation.AddError(exception);
            return operation;
        }

        var result = _mapper.Map<PersonViewModel>(entity);
        result.Age = _dateCalcualtorService.GetTotalYears(result.DateOfBirth);
        result.CountryName = country.Name;

        operation.Result = result;
        _logger.LogInformation("Person was created successfully");
        return operation;

    }
}
