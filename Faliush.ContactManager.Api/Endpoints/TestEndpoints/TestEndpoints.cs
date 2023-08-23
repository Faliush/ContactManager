using Faliush.ContactManager.Api.Definitions.Base;
using Faliush.ContactManager.Core.Test.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Faliush.ContactManager.Api.Endpoints.TestEndpoints;

public class TestEndpoints : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        app.MapGet("api/test/secret", GetTestSecret);
    }

    [Authorize]
    private async Task<string> GetTestSecret(IMediator mediator, HttpContext context) =>
        await mediator.Send(new SecretGetRequest(), context.RequestAborted);
    
}
