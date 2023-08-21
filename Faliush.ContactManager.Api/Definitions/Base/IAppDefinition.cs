namespace Faliush.ContactManager.Api.Definitions.Base;

public interface IAppDefinition
{
    void ConfigureServices(WebApplicationBuilder builder);

    void ConfigureApplication(WebApplication app);
}
