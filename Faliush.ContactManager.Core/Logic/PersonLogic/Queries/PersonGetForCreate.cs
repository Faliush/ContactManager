using AutoMapper;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using Faliush.ContactManager.Models.Base;
using MediatR;
using System.Security.Cryptography.X509Certificates;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetForCreateRequest() : IRequest<PersonCreateViewModel>;

public class PersonGetForCreateRequestHandler : IRequestHandler<PersonGetForCreateRequest, PersonCreateViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PersonGetForCreateRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<PersonCreateViewModel> Handle(PersonGetForCreateRequest request, CancellationToken cancellationToken)
    {
        var result = new PersonCreateViewModel();

        var items = await _unitOfWork.GetRepository<Country>()
            .GetAllAsync
            (
                disableTracking: true
            );

        if (items is null)
            throw new ContactManagerNotFoundException("Doesn't contain any countries");

        var countries = _mapper.Map<List<CountryViewModel>>(items.ToList());
        result.Countries = countries.ToList();
        result.Genders = Enum.GetNames<GenderOptions>().ToList();

        return result;
    }
}
