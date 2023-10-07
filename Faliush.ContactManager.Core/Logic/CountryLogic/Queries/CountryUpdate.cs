using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryUpdateRequest(CountryUpdateViewModel Model) : IRequest<OperationResult<CountryViewModel>>;

public class CountryUpdateRequestHandler : IRequestHandler<CountryUpdateRequest, OperationResult<CountryViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public CountryUpdateRequestHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;

    public async Task<OperationResult<CountryViewModel>> Handle(CountryUpdateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<CountryViewModel>();
        var repository = _unitOfWork.GetRepository<Country>();

        var item = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Name == request.Model.Name && x.Id != request.Model.Id
            );

        if(item is not null)
        {
            operation.AddError(new ContactManagerArgumentException($"country with name {request.Model.Name} already exist"));
            return operation;
        }

        var entity = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Model.Id
            );

        if (entity is null)
        {
            operation.AddError(new ContactManagerNotFoundException($"country with id: {request.Model.Id} not found"));
            return operation;
        }

        entity.Name = request.Model.Name;

        repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
        {
            var exception = _unitOfWork.LastSaveChangeResult.Exception ?? new ContactManagerSaveDatabaseException();
            operation.AddError(exception);
            return operation;
        }

        var result = new CountryViewModel()
        {
            Id= entity.Id,
            Name = entity.Name
        };

        operation.Result = result;

        return operation;
    }
}
