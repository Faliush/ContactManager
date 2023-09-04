using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Core.Services;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetByIdRequest(Guid Id) : IRequest<OperationResult<PersonViewModel>>;

public class PersonGetByIdRequestHandler : IRequestHandler<PersonGetByIdRequest, OperationResult<PersonViewModel>>
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

    public async Task<OperationResult<PersonViewModel>> Handle(PersonGetByIdRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<PersonViewModel>();
        var item = await _unitOfWork.GetRepository<Person>()
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id,
                disableTracking: true
            );

        if (item is null)
        {
            operation.AddError(new ContactManagerNotFoundException($"person with id: {request.Id} not found"));
            return operation;   
        }

        var result = _mapper.Map<PersonViewModel>(item);
        result.Age = _dateCalcualtorService.GetTotalYears(result.DateOfBirth);

        operation.Result = result;

        return operation;
    }
}
