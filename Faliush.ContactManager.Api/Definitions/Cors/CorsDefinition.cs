using Faliush.ContactManager.Api.Definitions.Base;

namespace Faliush.ContactManager.Api.Definitions.Cors;

public class CorsDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
        });
    }
}
