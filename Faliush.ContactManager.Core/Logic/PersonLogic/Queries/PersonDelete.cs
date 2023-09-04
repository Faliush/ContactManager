using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonDeleteRequest(Guid Id) : IRequest<OperationResult<Guid>>;

public class PersonDeleteRequestHandler : IRequestHandler<PersonDeleteRequest, OperationResult<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public PersonDeleteRequestHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    
    public async Task<OperationResult<Guid>> Handle(PersonDeleteRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<Guid>();    
        var repository = _unitOfWork.GetRepository<Person>();

        var entity = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id
            );

        if (entity is null)
        {
            operation.AddError(new ContactManagerNotFoundException($"peron with id: {request.Id} not found"));
            return operation;
        }

        repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
            throw new ContactManagerSaveDatabaseException("database saving error", _unitOfWork.LastSaveChangeResult.Exception);

        operation.Result = entity.Id;

        return operation;
    }
}
