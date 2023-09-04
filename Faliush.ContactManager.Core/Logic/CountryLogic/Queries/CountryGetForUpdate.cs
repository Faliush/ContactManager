using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryGetForUpdateRequest(Guid Id) : IRequest<OperationResult<CountryUpdateViewModel>>;

public class CountryGetForUpdateRequestHandler : IRequestHandler<CountryGetForUpdateRequest, OperationResult<CountryUpdateViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CountryGetForUpdateRequestHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    
    public async Task<OperationResult<CountryUpdateViewModel>> Handle(CountryGetForUpdateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<CountryUpdateViewModel>();
        var entity = await _unitOfWork.GetRepository<Country>()
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id,
                disableTracking: true
            );

        if (entity is null)
        {
            operation.AddError(new ContactManagerNotFoundException($"country with id: {request.Id} not found"));
            return operation;
        }

        var result = new CountryUpdateViewModel()
        {
            Id = entity.Id,
            Name = entity.Name
        };

        operation.Result = result;

        return operation;
    }
}
