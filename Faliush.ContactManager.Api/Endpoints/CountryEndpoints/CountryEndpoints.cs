using Faliush.ContactManager.Api.Definitions.Base;
using Faliush.ContactManager.Core.Logic.CountryLogic.Queries;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using MediatR;

namespace Faliush.ContactManager.Api.Endpoints.CountryEndpoints;

public class CountryEndpoints : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        app.MapGet("api/country/get-all", GetAllCountries);
        app.MapPost("api/country/create", CreateCountry);
        app.MapDelete("api/country/delete/{id:guid}", DeleteCountry);
        app.MapGet("api/country/update/{id:guid}", GetForUpdateCountry);
        app.MapPut("api/country/update", PutAfterUpdateCountry);
    }

    private async Task<List<CountryViewModel>> GetAllCountries(IMediator mediator, HttpContext context) =>
        await mediator.Send(new CountryGetAllRequest(), context.RequestAborted);

    private async Task<CountryViewModel> CreateCountry(IMediator mediator, CountryCreateViewModel viewModel, HttpContext context) =>
        await mediator.Send(new CountryCreateRequest(viewModel), context.RequestAborted);

    private async Task<Guid> DeleteCountry(Guid id, IMediator mediator, HttpContext context) =>
        await mediator.Send(new CountryDeleteRequest(id), context.RequestAborted);

    private async Task<CountryUpdateViewModel> GetForUpdateCountry(Guid id, IMediator mediator, HttpContext context) =>
        await mediator.Send(new CountryGetForUpdateRequest(id), context.RequestAborted);

    private async Task<CountryViewModel> PutAfterUpdateCountry(CountryUpdateViewModel viewModel, IMediator mediator, HttpContext context) =>
        await mediator.Send(new CountryUpdateRequest(viewModel), context.RequestAborted);
}
