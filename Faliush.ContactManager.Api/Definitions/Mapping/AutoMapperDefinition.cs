using Faliush.ContactManager.Api.Definitions.Base;
using Faliush.ContactManager.Core.Mappers;

namespace Faliush.ContactManager.Api.Definitions.Mapping;

public class AutoMapperDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(CountryMapperConfiguration));
    }

    public override void ConfigureApplication(WebApplication app)
    {
        var mapper = app.Services.GetRequiredService<AutoMapper.IConfigurationProvider>();

        if (app.Environment.IsDevelopment())
            mapper.AssertConfigurationIsValid(); // validate Mapper Configuration
        else
            mapper.CompileMappings();
    }
}
