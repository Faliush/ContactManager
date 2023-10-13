using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryDeleteRequest(Guid Id) : IRequest<OperationResult<Guid>>;

public class CountryDeleteRequestHandler : IRequestHandler<CountryDeleteRequest, OperationResult<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CountryDeleteRequestHandler> _logger;

    public CountryDeleteRequestHandler(IUnitOfWork unitOfWork, ILogger<CountryDeleteRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<OperationResult<Guid>> Handle(CountryDeleteRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<Guid>();
        var repository = _unitOfWork.GetRepository<Country>();
        _logger.LogInformation("CountryDeleteRequestHandler checks country id");

        var entity = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id
            );

        if (entity is null)
        {
            _logger.LogError("Given id doesn't exist in database");
            operation.AddError(new ContactManagerNotFoundException($"country with id: {request.Id} not found"));
            return operation;
        }

        repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
        {
            _logger.LogError("Arose exception during data saving");
            var exception = _unitOfWork.LastSaveChangeResult.Exception ?? new ContactManagerSaveDatabaseException();
            operation.AddError(exception);
            return operation;
        }

        operation.Result = entity.Id;
        _logger.LogInformation("Country was deleted successfyly");
        return operation;
    }
}
