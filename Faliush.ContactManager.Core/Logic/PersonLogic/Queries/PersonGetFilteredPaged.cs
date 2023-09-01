using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Enums;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Core.Services;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Infrastructure.UnitOfWork.Pagination;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetFilteredPagedRequest(
    int pageIndex, 
    int pageSize, 
    string? searchBy, 
    string? searchString, 
    string sortBy, 
    string sortOrder) 
        : IRequest<OperationResult<IPagedList<PeopleViewModel>>>;

public class PersonGetFilteredPagedRequestHandler : IRequestHandler<PersonGetFilteredPagedRequest, OperationResult<IPagedList<PeopleViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringConvertService _stringConvertService;

    public PersonGetFilteredPagedRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IStringConvertService stringConvertService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _stringConvertService = stringConvertService;
    }

    public async Task<OperationResult<IPagedList<PeopleViewModel>>> Handle(PersonGetFilteredPagedRequest request, CancellationToken cancellationToken)
    {
        var order = _stringConvertService.ConvertToEnum<SortOptions>(request.sortOrder);

        var items = await _unitOfWork.GetRepository<Person>()
            .GetPagedListAsync
            (
                predicate: PersonExpressions.SearchPredicate(request.searchBy, request.searchString),
                orderBy: PersonExpressions.OrderBy(request.sortBy, order),
                pageIndex: request.pageIndex,
                pageSize: request.pageSize,
                disableTracking: true
            );

        var result = _mapper.Map<IPagedList<PeopleViewModel>>(items);

        return OperationResult<IPagedList<PeopleViewModel>>.CreateResult(result); 
    }
}

