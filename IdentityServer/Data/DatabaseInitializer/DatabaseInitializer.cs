using IdentityServer.Data.Base;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data.DatabaseInitializer;

public static  class DatabaseInitializer
{
    public static async void SeedUser(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();
        await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        if (context!.Users.Any())
            return;

        var roles = AppData.Roles.ToList();

        foreach ( var role in roles )
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            if(!context!.Roles.Any(r => r.Name == role))
                await roleManager.CreateAsync(new ApplicationRole() { Name = role, NormalizedName = role.ToUpper() });
        }

        var developer = new ApplicationUser()
        {
            Email = "faliushdev@gmail.com",
            NormalizedEmail = "faliushdev@gmail.com".ToUpper(),
            UserName = "Developer",
            NormalizedUserName = "Developer".ToUpper(),
            PhoneNumber = "+380000000000",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if(!context!.Users.Any(u => u.Email == developer.Email))
        {
            var password = new PasswordHasher<ApplicationUser>();
            var hashed = password.HashPassword(developer, configuration["Secret:AdministratorPassword"]);
            developer.PasswordHash = hashed;
            var userStore = scope.ServiceProvider.GetRequiredService<ApplicationUserStore>();
            var resutl = await userStore.CreateAsync(developer);

            if(!resutl.Succeeded)
                throw new InvalidOperationException("Cannot create account");

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            foreach(var role in roles)
            {
                var roleAdded = await userManager.AddToRoleAsync(developer, role);
                if (roleAdded.Succeeded)
                    await context.SaveChangesAsync();
            }
        }
        
        await context.SaveChangesAsync();
        
        /// for IdentityServer

        scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

        var configurationContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        configurationContext.Database.Migrate();

        if (!configurationContext.Clients.Any())
        {
            foreach (var client in Configuration.GetClients())
            {
                configurationContext.Clients.Add(client.ToEntity());
            }
            configurationContext.SaveChanges();
        }

        if (!configurationContext.IdentityResources.Any())
        {
            foreach (var resource in Configuration.GetIdentityResources())
            {
                configurationContext.IdentityResources.Add(resource.ToEntity());
            }
            configurationContext.SaveChanges();
        }

        if (!configurationContext.ApiScopes.Any())
        {
            foreach (var resource in Configuration.GetApiScopes())
            {
                configurationContext.ApiScopes.Add(resource.ToEntity());
            }
            configurationContext.SaveChanges();
        }
    }
}
