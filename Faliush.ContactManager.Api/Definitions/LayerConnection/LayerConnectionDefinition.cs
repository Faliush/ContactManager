using Faliush.ContactManager.Api.Definitions.Base;
using Faliush.ContactManager.Core;
using Faliush.ContactManager.Infrastructure;

namespace Faliush.ContactManager.Api.Definition.LayerConnections;

public class LayerConnectionDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_API_HOST");
        var dbName = Environment.GetEnvironmentVariable("DB_API_NAME");
        var dbPort = Environment.GetEnvironmentVariable("DB_API_PORT");
        var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

        var connStr = $"Server={dbHost},{dbPort};Initial Catalog={dbName};User Id=sa;Password={dbPassword}";
        
        builder.Services.AddInfrastructureServices(connStr);
        builder.Services.AddCoreServices();
    }
}
