using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Enums;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetFilteredRequest(string? searchBy, string? searchString, string sortBy, SortOptions sortOrder) 
    : IRequest<OperationResult<List<PeopleViewModel>>>;


public class PersonGetFilteredRequestHandler : IRequestHandler<PersonGetFilteredRequest, OperationResult<List<PeopleViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public PersonGetFilteredRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult<List<PeopleViewModel>>> Handle(PersonGetFilteredRequest request, CancellationToken cancellationToken)
    {
        var items = await _unitOfWork.GetRepository<Person>()
            .GetAllAsync
            (
                predicate: PersonExpressions.SearchPredicate(request.searchBy, request.searchString),
                orderBy: PersonExpressions.OrderBy(request.sortBy, request.sortOrder)
            );

        var result = _mapper.Map<List<PeopleViewModel>>(items.ToList());

        return OperationResult<List<PeopleViewModel>>.CreateResult(result);
    }
}
