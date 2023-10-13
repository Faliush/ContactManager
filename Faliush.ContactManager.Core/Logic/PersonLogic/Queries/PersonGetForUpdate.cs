using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetForUpdateRequest(Guid Id) : IRequest<OperationResult<PersonUpdateViewModel>>;

public class PersonGetForUpdateRequestHandler : IRequestHandler<PersonGetForUpdateRequest, OperationResult<PersonUpdateViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PersonGetForUpdateRequestHandler> _logger;
    public PersonGetForUpdateRequestHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        ILogger<PersonGetForUpdateRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;    
        _logger = logger;
    }
    
    public async Task<OperationResult<PersonUpdateViewModel>> Handle(PersonGetForUpdateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<PersonUpdateViewModel>();
        _logger.LogInformation("PersongetForUpdateRequestHandler checks given id for existance in database");
        var entity = await _unitOfWork.GetRepository<Person>()
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id,
                disableTracking: true
            );

        if (entity is null)
        {
            _logger.LogError("Given person id doesn't exist in database");
            operation.AddError(new ContactManagerNotFoundException($"person with id: {request.Id} not found"));
            return operation;
        }

        var result = _mapper.Map<PersonUpdateViewModel>(entity);

        operation.Result = result;
        _logger.LogInformation("PersonGetForUpdateRequestHandler gave need person successfully");
        return operation;
    }
}
