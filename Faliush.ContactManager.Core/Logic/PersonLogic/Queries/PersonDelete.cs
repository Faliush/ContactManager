using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Services.Interfaces;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonDeleteRequest(Guid Id) : IRequest<OperationResult<Guid>>;

public class PersonDeleteRequestHandler : IRequestHandler<PersonDeleteRequest, OperationResult<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly ILogger<PersonDeleteRequestHandler> _logger;

    public PersonDeleteRequestHandler(
        IUnitOfWork unitOfWork, 
        ICacheService cacheService,
        ILogger<PersonDeleteRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _logger = logger;
    }
    
    public async Task<OperationResult<Guid>> Handle(PersonDeleteRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<Guid>();    
        var repository = _unitOfWork.GetRepository<Person>();

        _logger.LogInformation("PersonDeleteRequestHandler checks given person id for existance in database");
        var entity = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id
            );

        if (entity is null)
        {
            _logger.LogError("Given id doesn't exist in database");
            operation.AddError(new ContactManagerNotFoundException($"peron with id: {request.Id} not found"));
            return operation;
        }

        repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Delete person by given id");

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
        {
            _logger.LogError("Arose exception during data saving");
            var exception = _unitOfWork.LastSaveChangeResult.Exception ?? new ContactManagerSaveDatabaseException();
            operation.AddError(exception);
            return operation;
        }

        _logger.LogInformation("PersonDeleteRequestHandler removes person by id from cache");
        await _cacheService.RemoveAsync($"person-{entity.Id}", cancellationToken);
        await _cacheService.RemoveByPrefixAsync("people-filtered", cancellationToken);

        operation.Result = entity.Id;
        _logger.LogInformation("Person was deleted successfully"); 
        return operation;
    }
}
