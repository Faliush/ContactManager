using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryDeleteRequest(Guid Id) : IRequest<OperationResult<Guid>>;

public class CountryDeleteRequestHandler : IRequestHandler<CountryDeleteRequest, OperationResult<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CountryDeleteRequestHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    
    public async Task<OperationResult<Guid>> Handle(CountryDeleteRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<Guid>();
        var repository = _unitOfWork.GetRepository<Country>();

        var entity = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id
            );

        if (entity is null)
        {
            operation.AddError(new ContactManagerNotFoundException($"country with id: {request.Id} not found"));
            return operation;
        }

        repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
        {
            var exception = _unitOfWork.LastSaveChangeResult.Exception ?? new ContactManagerSaveDatabaseException();
            operation.AddError(exception);
            return operation;
        }

        operation.Result = entity.Id;

        return operation;
    }
}
