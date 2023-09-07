using Faliush.ContactManager.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Faliush.ContactManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            option => option.UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDbContext))));

        services.AddScoped<IRepositoryFactory, UnitOfWork<ApplicationDbContext>>();
        services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();
        services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();

        return services;
    }
}
