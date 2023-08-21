namespace Faliush.ContactManager.Api.Definitions.Base;

public abstract class AppDefinition : IAppDefinition
{
    public virtual void ConfigureApplication(WebApplication app) {  }

    public virtual void ConfigureServices(WebApplicationBuilder builder) {  }
}
