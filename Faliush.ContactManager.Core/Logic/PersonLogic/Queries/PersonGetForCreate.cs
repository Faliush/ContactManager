using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using Faliush.ContactManager.Models.Base;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetForCreateRequest() : IRequest<OperationResult<PersonCreateViewModel>>;

public class PersonGetForCreateRequestHandler : IRequestHandler<PersonGetForCreateRequest, OperationResult<PersonCreateViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PersonGetForCreateRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<OperationResult<PersonCreateViewModel>> Handle(PersonGetForCreateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<PersonCreateViewModel>();
        var result = new PersonCreateViewModel();

        var items = await _unitOfWork.GetRepository<Country>()
            .GetAllAsync
            (
                disableTracking: true
            );

        if (items is null)
        {
            operation.AddError(new ContactManagerNotFoundException("Doesn't contain any countries"));
            return operation;
        }

        var countries = _mapper.Map<List<CountryViewModel>>(items.ToList());
        result.Countries = countries.ToList();
        result.Genders = Enum.GetNames<GenderOptions>().ToList();

        operation.Result = result;

        return operation;
    }
}
