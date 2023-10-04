using IdentityServer.Data;
using IdentityServer.Data.Base;
using IdentityServer.Services;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Definitions;

public static class DbConnectionDefinition 
{
    public static IServiceCollection AddDbConnections(this IServiceCollection services)
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_AUTH_HOST") ?? "localhost";
        var dbName = Environment.GetEnvironmentVariable("DB_AUTH_NAME") ?? "ContactManagerIdentityServerDb";
        var dbPort = Environment.GetEnvironmentVariable("DB_AUTH_PORT") ?? "5432";
        var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD") ?? "12345";

        var connectionString = $"Server={dbHost};Port={dbPort};Database={dbName};User ID=postgres;Password={dbPassword}";
        var migrationAssembly = typeof(Program).Assembly.GetName().Name;

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        })
            .AddIdentity<ApplicationUser, ApplicationRole>(config =>
            {
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequiredLength = 5;
                config.Password.RequiredUniqueChars = 3;
                config.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer(config =>
        {
            config.UserInteraction.LoginUrl = "/Account/Login";
        })
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<ProfileService>()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext =
                    b => b.UseNpgsql(connectionString,
                        sql => sql.MigrationsAssembly(migrationAssembly));

            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext =
                    b => b.UseNpgsql(connectionString,
                        sql => sql.MigrationsAssembly(migrationAssembly));
            })
            .AddDeveloperSigningCredential();


        return services;
    }
}
