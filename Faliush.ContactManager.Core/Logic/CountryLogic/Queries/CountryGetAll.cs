using AutoMapper;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Core.Services.Interfaces;
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
    private readonly ICacheService _cacheService;
    private readonly ILogger<CountryGetAllRequestHandler> _logger;

    public CountryGetAllRequestHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        ICacheService cacheService,
        ILogger<CountryGetAllRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<OperationResult<List<CountryViewModel>>> Handle(CountryGetAllRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<List<CountryViewModel>>();

        _logger.LogInformation("CountryGetAllRequestHandler checks list of countries for existance in cache");
        var cachedValues = await _cacheService.GetAsync<List<Country>>("countries", cancellationToken);

        if(cachedValues is not null)
        {
            _logger.LogInformation("Give list of countries from cache");
            operation.Result = _mapper.Map<List<CountryViewModel>>(cachedValues); 
            return operation;
        }
            
        var items = await _unitOfWork.GetRepository<Country>()
            .GetAllAsync();

        _logger.LogInformation("CountryGetAllRequestHandler sets the list of countris in the cache");
        await _cacheService.SetAsync("countries", items.ToList(), cancellationToken);
        
        var result = _mapper.Map<List<CountryViewModel>>(items.ToList());

        operation.Result = result;
        _logger.LogInformation("CountryGetAllRequestHandler gave all country from database successfully");
        return operation;
    }
}
