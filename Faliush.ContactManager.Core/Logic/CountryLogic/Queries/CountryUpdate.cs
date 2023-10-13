using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryUpdateRequest(CountryUpdateViewModel Model) : IRequest<OperationResult<CountryViewModel>>;

public class CountryUpdateRequestHandler : IRequestHandler<CountryUpdateRequest, OperationResult<CountryViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CountryUpdateRequestHandler> _logger;

    public CountryUpdateRequestHandler(IUnitOfWork unitOfWork, ILogger<CountryUpdateRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<OperationResult<CountryViewModel>> Handle(CountryUpdateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<CountryViewModel>();
        var repository = _unitOfWork.GetRepository<Country>();

        _logger.LogInformation("CountryUpdateRequestHandler checks name for existance in database");
        var item = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Name == request.Model.Name && x.Id != request.Model.Id
            );

        if(item is not null)
        {
            _logger.LogError("Given name already exist in database");
            operation.AddError(new ContactManagerArgumentException($"country with name {request.Model.Name} already exist"));
            return operation;
        }

        _logger.LogInformation("CountryUpdateRequestHandler checks given id for existence in database");
        var entity = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Model.Id
            );

        if (entity is null)
        {
            _logger.LogError("Country with given id doesn't exist");
            operation.AddError(new ContactManagerNotFoundException($"country with id: {request.Model.Id} not found"));
            return operation;
        }

        entity.Name = request.Model.Name;

        repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Update given country");

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
        {
            _logger.LogError("Arose exception during data saving");
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
        _logger.LogInformation("Country was updated successfully");
        return operation;
    }
}
