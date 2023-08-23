using Faliush.ContactManager.Api.Definitions.Base;

namespace Faliush.ContactManager.Api.Definitions.Mapping;

public class AutoMapperDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(Program));
    }

    public override void ConfigureApplication(WebApplication app)
    {
        var mapper = app.Services.GetRequiredService<AutoMapper.IConfigurationProvider>();
        
        if (app.Environment.IsDevelopment())
            mapper.AssertConfigurationIsValid();// validate Mapper Configuration
        else
            mapper.CompileMappings();
    }
}
