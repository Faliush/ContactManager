using IdentityServer.Data.Base;
using Microsoft.AspNetCore.Identity;

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
    }
}
