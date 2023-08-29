using AutoMapper;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Core.Services;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using Faliush.ContactManager.Models.Base;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonCreateRequest(PersonCreateViewModel Model, ClaimsPrincipal User) : IRequest<PersonViewModel>;

public class PersonCreateRequestHandler : IRequestHandler<PersonCreateRequest, PersonViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringConvertService _stringConvertService;
    private readonly IDateCalcualtorService _dateCalcualtorService;

    public PersonCreateRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, 
        IStringConvertService stringConvertService,
        IDateCalcualtorService dateCalcualtorService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _stringConvertService = stringConvertService;
        _dateCalcualtorService = dateCalcualtorService;
    }

    public async Task<PersonViewModel> Handle(PersonCreateRequest request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Person>();

        var entity = _mapper.Map<Person>(request.Model, x => x.Items[nameof(IdentityUser)] = request.User.Identity!.Name);
        entity.Gender = _stringConvertService.ConvertToEnum<GenderOptions>(request.Model.Gender);

        var country = await _unitOfWork.GetRepository<Country>()
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Model.CountryId
            );

        if (country is null)
            throw new ContactManagerNotFoundException($"country with id {request.Model.CountryId} doesn't exist");

        await repository.InsertAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
            throw new ContactManagerSaveDatabaseException();

        var result = _mapper.Map<PersonViewModel>(entity);
        result.Age = _dateCalcualtorService.GetTotalYears(result.DateOfBirth);
        result.CountryName = country.Name;

        return result;

    }
}
