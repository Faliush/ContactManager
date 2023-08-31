using AutoMapper;
using Faliush.ContactManager.Core.Enums;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Core.Services;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetFilteredRequest(string? searchBy, string? searchString, string sortBy, string sortOrder) 
    : IRequest<List<PeopleViewModel>>;


public class PersonGetFilteredRequestHandler : IRequestHandler<PersonGetFilteredRequest, List<PeopleViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringConvertService _stringConvertService;
    public PersonGetFilteredRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IStringConvertService stringConvertService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _stringConvertService = stringConvertService;
    }

    public async Task<List<PeopleViewModel>> Handle(PersonGetFilteredRequest request, CancellationToken cancellationToken)
    {
        var order = _stringConvertService.ConvertToEnum<SortOptions>(request.sortOrder);

        var items = await _unitOfWork.GetRepository<Person>()
            .GetAllAsync
            (
                predicate: PersonExpressions.SearchPredicate(request.searchBy, request.searchString),
                orderBy: PersonExpressions.OrderBy(request.sortBy, order)
            );

        var result = _mapper.Map<List<PeopleViewModel>>(items.ToList());

        return result;
    }
}
