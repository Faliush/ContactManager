using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryCreateRequest(CountryCreateViewModel Model) : IRequest<CountryViewModel>;

public class CountryCreateRequestHandler : IRequestHandler<CountryCreateRequest, CountryViewModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public CountryCreateRequestHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    
    public async Task<CountryViewModel> Handle(CountryCreateRequest request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Country>();

        var entity = new Country
        {
            Name = request.Model.Name
        };

        await repository.InsertAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
            throw new ContactManagerSaveDatabaseException();

        var result = new CountryViewModel
        {
            Id = entity.Id,
            Name = entity.Name
        };

        return result;
    }
}

