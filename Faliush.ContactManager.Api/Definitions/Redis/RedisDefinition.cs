using Faliush.ContactManager.Api.Definitions.Base;

namespace Faliush.ContactManager.Api.Definitions.Redis;

public class RedisDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
        });
    }
}
