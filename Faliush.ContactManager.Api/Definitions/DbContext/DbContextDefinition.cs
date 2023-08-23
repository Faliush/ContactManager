using Faliush.ContactManager.Api.Definitions.Base;
using Faliush.ContactManager.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Faliush.ContactManager.Api.Definitions.DbContext;

public class DbContextDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(
            option => option.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ApplicationDbContext))));
    }
}
