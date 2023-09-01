using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryCreateRequest(CountryCreateViewModel Model) : IRequest<OperationResult<CountryViewModel>>;

public class CountryCreateRequestHandler : IRequestHandler<CountryCreateRequest, OperationResult<CountryViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CountryCreateRequestHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    
    public async Task<OperationResult<CountryViewModel>> Handle(CountryCreateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<CountryViewModel>();
        var repository = _unitOfWork.GetRepository<Country>();

        var entity = new Country
        {
            Name = request.Model.Name
        };

        await repository.InsertAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
        {
            var exception = _unitOfWork.LastSaveChangeResult.Exception ?? new ContactManagerSaveDatabaseException();
            operation.AddError(exception);
            return operation;
        }

        var result = new CountryViewModel
        {
            Id = entity.Id,
            Name = entity.Name
        };

        operation.Result = result;
        return operation;
    }
}

