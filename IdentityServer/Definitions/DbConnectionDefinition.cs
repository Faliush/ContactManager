using IdentityServer.Data;
using IdentityServer.Data.Base;
using IdentityServer.Services;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Definitions;

public static class DbConnectionDefinition 
{
    public static IServiceCollection AddDbConnections(this IServiceCollection services)
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_AUTH_HOST");
        var dbName = Environment.GetEnvironmentVariable("DB_AUTH_NAME");
        var dbPort = Environment.GetEnvironmentVariable("DB_AUTH_PORT");
        var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

        var connectionString = $"Server={dbHost},{dbPort};Initial Catalog={dbName};User Id=sa;Password={dbPassword}";
        var migrationAssembly = typeof(Program).Assembly.GetName().Name;

        

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
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
                    b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationAssembly));

            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext =
                    b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationAssembly));
            })
            .AddDeveloperSigningCredential();


        return services;
    }
}
