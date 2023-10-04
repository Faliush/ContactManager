using Faliush.ContactManager.Api.Definitions.Base;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Faliush.ContactManager.Api.Definitions.HealthChecks;

public class HeathChecksDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        //builder.Services.AddHealthChecks(); TODO
    }

    public override void ConfigureApplication(WebApplication app)
    {
        //app.MapHealthChecks("/healthz");
        
    }
}
