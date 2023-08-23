using Faliush.ContactManager.Api.Definitions.Base;
using Faliush.ContactManager.Infrastructure;
using Faliush.ContactManager.Infrastructure.UnitOfWork;

namespace Faliush.ContactManager.Api.Definitions.DependencyContainer;

public class DependencyContainerDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        // UnitOfWork
        builder.Services.AddScoped<IRepositoryFactory, UnitOfWork<ApplicationDbContext>>();
        builder.Services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();
    }
}
