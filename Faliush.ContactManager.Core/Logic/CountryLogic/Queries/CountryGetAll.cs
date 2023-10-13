using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryGetAllRequest() : IRequest<OperationResult<List<CountryViewModel>>>;

public class CountryGetAllRequestHandler : IRequestHandler<CountryGetAllRequest, OperationResult<List<CountryViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CountryGetAllRequestHandler> _logger;

    public CountryGetAllRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CountryGetAllRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<OperationResult<List<CountryViewModel>>> Handle(CountryGetAllRequest request, CancellationToken cancellationToken)
    {

        var operation = new OperationResult<List<CountryViewModel>>();


        var items = await _unitOfWork.GetRepository<Country>()
            .GetAllAsync();

        var result = _mapper.Map<List<CountryViewModel>>(items.ToList());

        operation.Result = result;
        _logger.LogInformation("CountryGetAllRequestHandler gave all country from database successfully");
        return operation;
    }
}
