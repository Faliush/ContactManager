using Faliush.ContactManager.Api.Definitions.Base;
using Serilog;

namespace Faliush.ContactManager.Api.Definitions.Serilog;

public class SerilogDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider provider, LoggerConfiguration configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(provider);
        });
    }

}
