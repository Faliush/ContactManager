using Faliush.ContactManager.Api.Definitions.Base;
using Faliush.ContactManager.Core;
using Faliush.ContactManager.Infrastructure;

namespace Faliush.ContactManager.Api.Definition.LayerConnections;

public class LayerConnectionDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_API_HOST") ?? "localhost";
        var dbName = Environment.GetEnvironmentVariable("DB_API_NAME") ?? "ContactManagerApiDb";
        var dbPort = Environment.GetEnvironmentVariable("DB_API_PORT") ?? "5432";
        var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD") ?? "12345";

        var connStr = $"Server={dbHost};Port={dbPort};Database={dbName};User ID=postgres;Password={dbPassword}";
        
        builder.Services.AddInfrastructureServices(connStr);
        builder.Services.AddCoreServices();
    }
}
