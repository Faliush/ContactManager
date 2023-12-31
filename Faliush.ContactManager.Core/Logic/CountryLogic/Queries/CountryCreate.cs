﻿using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Exceptions;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Core.Services.Interfaces;
using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Faliush.ContactManager.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Queries;

public record CountryCreateRequest(CountryCreateViewModel Model) : IRequest<OperationResult<CountryViewModel>>;

public class CountryCreateRequestHandler : IRequestHandler<CountryCreateRequest, OperationResult<CountryViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly ILogger<CountryCreateRequestHandler> _logger;

    public CountryCreateRequestHandler(
        IUnitOfWork unitOfWork, 
        ICacheService cacheService,
        ILogger<CountryCreateRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _logger = logger;
    }
    
    public async Task<OperationResult<CountryViewModel>> Handle(CountryCreateRequest request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult<CountryViewModel>();
        var repository = _unitOfWork.GetRepository<Country>();
        _logger.LogInformation("CountryCreateRequestHandler checks the database for existence country name");

        var item = await repository.GetFirstOrDefaultAsync
            (
                predicate: x => x.Name == request.Model.Name
            );

        if (item is not null)
        {
            _logger.LogError("Country name already exists in database");
            operation.AddError(new ContactManagerArgumentException($"country with name {request.Model.Name} already exists"));
            return operation;
        }

        var entity = new Country
        {
            Name = request.Model.Name
        };

        _logger.LogInformation("Add country to the database");
        await repository.InsertAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangeResult.IsOk)
        {
            _logger.LogError("Arose exception during data saving");
            var exception = _unitOfWork.LastSaveChangeResult.Exception ?? new ContactManagerSaveDatabaseException();
            operation.AddError(exception);
            return operation;
        }

        _logger.LogInformation("Add country to cache");
        await _cacheService.SetAsync($"country-{entity.Id}", entity, cancellationToken);
        await _cacheService.RemoveAsync("countries", cancellationToken);

        var result = new CountryViewModel
        {
            Id = entity.Id,
            Name = entity.Name
        };

        operation.Result = result;
        _logger.LogInformation("Country was created successfully");
        return operation;
    }
}

