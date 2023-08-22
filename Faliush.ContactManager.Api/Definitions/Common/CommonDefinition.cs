using Faliush.ContactManager.Api.Definitions.Base;

namespace Faliush.ContactManager.Api.Definitions.Common;

public class CommonDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
    }

    public override void ConfigureApplication(WebApplication app)
    {
        app.MapControllers();
    }
}
