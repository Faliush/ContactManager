using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryUpdateRequest(CountryUpdateViewModel Model) : IRequest<CountryViewModel>;

public class CountryUpdateRequestHandler : IRequestHandler<CountryUpdateRequest, CountryViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    public CountryUpdateRequestHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;

    public async Task<CountryViewModel> Handle(CountryUpdateRequest request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Country>();

        var entity = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Model.Id
            );

        if (entity is null)
            throw new ContactManagerNotFoundException($"country with id: {request.Model.Id} not found");

        entity.Name = request.Model.Name;

        repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
            throw new ContactManagerSaveDatabaseException(_unitOfWork.LastSaveChangeResult.Exception?.ToString());

        var result = new CountryViewModel()
        {
            Id= entity.Id,
            Name = entity.Name
        };

        return result;
    }
}
