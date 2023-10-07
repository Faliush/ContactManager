using Faliush.ContactManager.Api.Definitions.Base;

namespace Faliush.ContactManager.Api.Definitions.ApplicationContext;

public class AppContextDefinition : AppDefinition
{
    public override void ConfigureApplication(WebApplication app) =>
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

}
