using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryGetForUpdateRequest(Guid Id) : IRequest<CountryUpdateViewModel>;

public class CountryGetForUpdateRequestHandler : IRequestHandler<CountryGetForUpdateRequest, CountryUpdateViewModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public CountryGetForUpdateRequestHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    
    public async Task<CountryUpdateViewModel> Handle(CountryGetForUpdateRequest request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.GetRepository<Country>()
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id,
                disableTracking: true
            );

        if (entity is null)
            throw new ContactManagerNotFoundException($"country with id: {request.Id} not found");

        var result = new CountryUpdateViewModel()
        {
            Id = entity.Id,
            Name = entity.Name
        };

        return result;
    }
}
