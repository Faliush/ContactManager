﻿using Faliush.ContactManager.Api.Definitions.Base;

namespace Faliush.ContactManager.Api.Definitions.Redis;

public class RedisDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            var configuration = Environment.GetEnvironmentVariable("REDIS_PORT") 
                ?? builder.Configuration.GetConnectionString("Redis");

            options.Configuration = configuration;
            options.InstanceName = "ContactManagetApi";
        });
    }
}
