using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryDeleteRequest(Guid Id) : IRequest<Guid>;

public class CountryDeleteRequestHandler : IRequestHandler<CountryDeleteRequest, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CountryDeleteRequestHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    
    public async Task<Guid> Handle(CountryDeleteRequest request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Country>();

        var entity = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id
            );

        if (entity is null)
            throw new ContactManagerNotFoundException($"country with id: {request.Id} not found");

        repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
            throw new ContactManagerSaveDatabaseException();

        return entity.Id;
    }
}
