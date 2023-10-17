using Faliush.ContactManager.Api.Definitions.Base;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Faliush.ContactManager.Api.Definitions.HealthChecks;

public class HeathChecksDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();
    }

    public override void ConfigureApplication(WebApplication app)
    {
        
    }
}
