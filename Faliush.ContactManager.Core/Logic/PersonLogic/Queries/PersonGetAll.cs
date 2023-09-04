using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetAllRequest() : IRequest<OperationResult<List<PeopleViewModel>>>;

public class PersonGetAllRequestHandler : IRequestHandler<PersonGetAllRequest, OperationResult<List<PeopleViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PersonGetAllRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
        
    public async Task<OperationResult<List<PeopleViewModel>>> Handle(PersonGetAllRequest request, CancellationToken cancellationToken)
    {
        var items = await _unitOfWork.GetRepository<Person>()
            .GetAllAsync
            (
                disableTracking: true
            );

        var result = _mapper.Map<List<PeopleViewModel>>(items.ToList());

        return OperationResult<List<PeopleViewModel>>.CreateResult(result);
    }
}
