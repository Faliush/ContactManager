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
    }


    private async Task<List<CountryViewModel>> GetAllCountries(IMediator mediator, HttpContext context) =>
        await mediator.Send(new CountryGetAllRequest(), context.RequestAborted);

    private async Task<CountryViewModel> CreateCountry(IMediator mediator, CountryCreateViewModel viewModel, HttpContext context) =>
        await mediator.Send(new CountryCreateRequest(viewModel), context.RequestAborted);
}
