using AutoMapper;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Queries;

public record PersonGetForUpdateRequest(Guid Id) : IRequest<PersonUpdateViewModel>;

public class PersonGetForUpdateRequestHandler : IRequestHandler<PersonGetForUpdateRequest, PersonUpdateViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public PersonGetForUpdateRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;    
    }
    
    public async Task<PersonUpdateViewModel> Handle(PersonGetForUpdateRequest request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.GetRepository<Person>()
            .GetFirstOrDefaultAsync
            (
                predicate: x => x.Id == request.Id,
                disableTracking: true
            );

        if (entity is null)
            throw new ContactManagerNotFoundException($"person with id: {request.Id} not found");

        var result = _mapper.Map<PersonUpdateViewModel>(entity);

        return result;
    }
}
