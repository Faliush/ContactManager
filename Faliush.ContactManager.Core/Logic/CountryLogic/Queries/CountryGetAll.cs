using AutoMapper;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryGetAllRequest() : IRequest<List<CountryViewModel>>;

public class CountryGetAllRequestHandler : IRequestHandler<CountryGetAllRequest, List<CountryViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CountryGetAllRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CountryViewModel>> Handle(CountryGetAllRequest request, CancellationToken cancellationToken)
    {
        var items = await _unitOfWork.GetRepository<Country>()
            .GetAllAsync();

        var result = _mapper.Map<List<CountryViewModel>>(items.ToList());

        return result;
    }
}
