using IdentityServer.Data.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IdentityServer.Data;

public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>
{
    public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber describer) : base(context, describer)
    {
    }
}
