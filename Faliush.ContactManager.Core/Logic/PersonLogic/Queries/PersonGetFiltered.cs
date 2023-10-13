using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Enums;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetFilteredRequest(string? searchBy, string? searchString, string sortBy, SortOptions sortOrder) 
    : IRequest<OperationResult<List<PeopleViewModel>>>;


public class PersonGetFilteredRequestHandler : IRequestHandler<PersonGetFilteredRequest, OperationResult<List<PeopleViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PersonGetFilteredRequestHandler> _logger;
    public PersonGetFilteredRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<PersonGetFilteredRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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

        _logger.LogInformation("PersonGetFilteredRequest gave all person from database");
        return OperationResult<List<PeopleViewModel>>.CreateResult(result);
    }
}
