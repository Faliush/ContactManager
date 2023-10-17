using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Core.Services.Interfaces;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetByIdRequest(Guid Id) : IRequest<OperationResult<PersonViewModel>>;

public class PersonGetByIdRequestHandler : IRequestHandler<PersonGetByIdRequest, OperationResult<PersonViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateCalcualtorService _dateCalcualtorService;
    private readonly ICacheService _cacheService;
    private readonly ILogger<PersonGetByIdRequestHandler> _logger;

    public PersonGetByIdRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IDateCalcualtorService dateCalcualtorService,
        ICacheService cacheService,
        ILogger<PersonGetByIdRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateCalcualtorService = dateCalcualtorService;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<OperationResult<PersonViewModel>> Handle(PersonGetByIdRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<PersonViewModel>();

        _logger.LogInformation("PersonGetByIdRequestHandler checks given id for existance in cache");
        var cachedValue = await _cacheService.GetAsync<Person>($"person-{request.Id}", cancellationToken);
        
        if (cachedValue is not null)
        {
            _logger.LogInformation("PersonGetByIdRequestHandler gave need person by given id from cache");
            var response = _mapper.Map<PersonViewModel>(cachedValue);
            response.Age = _dateCalcualtorService.GetTotalYears(response.DateOfBirth);
            operation.Result = response;
            return operation;
        }

        _logger.LogInformation("PersonGetByIdRequestHandler checks given id for existance in database");
        var item = await _unitOfWork.GetRepository<Person>()
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id,
                disableTracking: true
            );

        if (item is null)
        {
            _logger.LogError("Given person id doens't exist in database");
            operation.AddError(new ContactManagerNotFoundException($"person with id: {request.Id} not found"));
            return operation;   
        }

        await _cacheService.SetAsync($"person-{item.Id}", item, cancellationToken);

        var result = _mapper.Map<PersonViewModel>(item);
        result.Age = _dateCalcualtorService.GetTotalYears(result.DateOfBirth);

        operation.Result = result;
        _logger.LogInformation("PersonGetByIdRequestHandler gave need person by given id");
        return operation;
    }
}
