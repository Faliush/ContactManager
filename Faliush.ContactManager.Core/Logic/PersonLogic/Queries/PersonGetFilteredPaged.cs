﻿using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Enums;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Infrastructure.UnitOfWork.Pagination;
using Faliush.ContactManager.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetFilteredPagedRequest(
    int pageIndex, 
    int pageSize, 
    string? searchBy, 
    string? searchString, 
    string sortBy, 
    SortOptions sortOrder) 
        : IRequest<OperationResult<IPagedList<PeopleViewModel>>>;

public class PersonGetFilteredPagedRequestHandler : IRequestHandler<PersonGetFilteredPagedRequest, OperationResult<IPagedList<PeopleViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PersonGetFilteredPagedRequestHandler> _logger;

    public PersonGetFilteredPagedRequestHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ILogger<PersonGetFilteredPagedRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<OperationResult<IPagedList<PeopleViewModel>>> Handle(PersonGetFilteredPagedRequest request, CancellationToken cancellationToken)
    {
        var items = await _unitOfWork.GetRepository<Person>()
            .GetPagedListAsync
            (
                predicate: PersonExpressions.SearchPredicate(request.searchBy, request.searchString),
                orderBy: PersonExpressions.OrderBy(request.sortBy, request.sortOrder),
                pageIndex: request.pageIndex,
                pageSize: request.pageSize,
                disableTracking: true
            );

        var result = _mapper.Map<IPagedList<PeopleViewModel>>(items);

        _logger.LogInformation($"PersonGetFilteredPagedRequestHandler gave persons on page {items.PageIndex}");
        return OperationResult<IPagedList<PeopleViewModel>>.CreateResult(result); 
    }
}

