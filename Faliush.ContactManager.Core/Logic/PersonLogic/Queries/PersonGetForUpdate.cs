using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetForUpdateRequest(Guid Id) : IRequest<OperationResult<PersonUpdateViewModel>>;

public class PersonGetForUpdateRequestHandler : IRequestHandler<PersonGetForUpdateRequest, OperationResult<PersonUpdateViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public PersonGetForUpdateRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;    
    }
    
    public async Task<OperationResult<PersonUpdateViewModel>> Handle(PersonGetForUpdateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<PersonUpdateViewModel>();
        var entity = await _unitOfWork.GetRepository<Person>()
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id,
                disableTracking: true
            );

        if (entity is null)
        {
            operation.AddError(new ContactManagerNotFoundException($"person with id: {request.Id} not found"));
            return operation;
        }

        var result = _mapper.Map<PersonUpdateViewModel>(entity);

        operation.Result = result;
        return operation;
    }
}
