using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryGetAllRequest() : IRequest<OperationResult<List<CountryViewModel>>>;

public class CountryGetAllRequestHandler : IRequestHandler<CountryGetAllRequest, OperationResult<List<CountryViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CountryGetAllRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult<List<CountryViewModel>>> Handle(CountryGetAllRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<List<CountryViewModel>>();


        var items = await _unitOfWork.GetRepository<Country>()
            .GetAllAsync();

        var result = _mapper.Map<List<CountryViewModel>>(items.ToList());

        operation.Result = result;

        return operation;
    }
}
