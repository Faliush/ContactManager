using Faliush.ContactManager.Api.Definitions.Base;
using Faliush.ContactManager.Core.Logic.CountryLogic.Queries;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Faliush.ContactManager.Api.Endpoints.CountryEndpoints;

public class CountryEndpoints : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        app.MapGet("api/countries", GetAllCountries);
        app.MapPost("api/countries", CreateCountry);
        app.MapDelete("api/countries/{id:guid}", DeleteCountry);
        app.MapGet("api/countries/update/{id:guid}", GetForUpdateCountry);
        app.MapPut("api/countries", PutAfterUpdateCountry);
    }

    [Authorize(Policy = "Administrator")]
    private async Task<List<CountryViewModel>> GetAllCountries(IMediator mediator, HttpContext context) =>
        await mediator.Send(new CountryGetAllRequest(), context.RequestAborted);

    [Authorize(Policy = "Administrator")]
    private async Task<CountryViewModel> CreateCountry(IMediator mediator, CountryCreateViewModel viewModel, HttpContext context) =>
        await mediator.Send(new CountryCreateRequest(viewModel), context.RequestAborted);

    [Authorize(Policy = "Administrator")]
    private async Task<Guid> DeleteCountry(Guid id, IMediator mediator, HttpContext context) =>
        await mediator.Send(new CountryDeleteRequest(id), context.RequestAborted);

    [Authorize(Policy = "Administrator")]
    private async Task<CountryUpdateViewModel> GetForUpdateCountry(Guid id, IMediator mediator, HttpContext context) =>
        await mediator.Send(new CountryGetForUpdateRequest(id), context.RequestAborted);

    [Authorize(Policy = "Administrator")]
    private async Task<CountryViewModel> PutAfterUpdateCountry(CountryUpdateViewModel viewModel, IMediator mediator, HttpContext context) =>
        await mediator.Send(new CountryUpdateRequest(viewModel), context.RequestAborted);
}
