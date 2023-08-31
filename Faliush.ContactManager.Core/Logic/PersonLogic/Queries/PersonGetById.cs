using AutoMapper;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Core.Services;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetByIdRequest(Guid Id) : IRequest<PersonViewModel>;

public class PersonGetByIdRequestHandler : IRequestHandler<PersonGetByIdRequest, PersonViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateCalcualtorService _dateCalcualtorService;

    public PersonGetByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IDateCalcualtorService dateCalcualtorService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateCalcualtorService = dateCalcualtorService;
    }

    public async Task<PersonViewModel> Handle(PersonGetByIdRequest request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.GetRepository<Person>()
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id,
                disableTracking: true
            );

        if (item is null)
            throw new ContactManagerNotFoundException($"person with id: {request.Id} not found");

        var result = _mapper.Map<PersonViewModel>(item);

        result.Age = _dateCalcualtorService.GetTotalYears(result.DateOfBirth);

        return result;
    }
}
