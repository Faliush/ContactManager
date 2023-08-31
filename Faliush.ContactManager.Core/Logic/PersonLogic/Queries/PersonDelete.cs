using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonDeleteRequest(Guid Id) : IRequest<Guid>;

public class PersonDeleteRequestHandler : IRequestHandler<PersonDeleteRequest, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public PersonDeleteRequestHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    
    public async Task<Guid> Handle(PersonDeleteRequest request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Person>();

        var entity = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id
            );

        if (entity is null)
            throw new ContactManagerNotFoundException($"peron with id: {request.Id} not found");

        repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
            throw new ContactManagerSaveDatabaseException("database saving error", _unitOfWork.LastSaveChangeResult.Exception);

        return entity.Id;
    }
}
