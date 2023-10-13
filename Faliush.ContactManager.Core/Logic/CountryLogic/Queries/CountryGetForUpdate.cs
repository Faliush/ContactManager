using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryGetForUpdateRequest(Guid Id) : IRequest<OperationResult<CountryUpdateViewModel>>;

public class CountryGetForUpdateRequestHandler : IRequestHandler<CountryGetForUpdateRequest, OperationResult<CountryUpdateViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CountryGetForUpdateRequestHandler> _logger;

    public CountryGetForUpdateRequestHandler(IUnitOfWork unitOfWork, ILogger<CountryGetForUpdateRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<OperationResult<CountryUpdateViewModel>> Handle(CountryGetForUpdateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<CountryUpdateViewModel>();
        _logger.LogInformation("CountryGetForUpdateRequestHandler checks country id for existence in database");

        var entity = await _unitOfWork.GetRepository<Country>()
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id,
                disableTracking: true
            );

        if (entity is null)
        {
            _logger.LogError("Given id doesn't exist in database");
            operation.AddError(new ContactManagerNotFoundException($"country with id: {request.Id} not found"));
            return operation;
        }

        var result = new CountryUpdateViewModel()
        {
            Id = entity.Id,
            Name = entity.Name
        };

        operation.Result = result;
        _logger.LogInformation("CountryGetForUpdateRequestHandler get nedded country for update from database successfully");
        return operation;
    }
}
