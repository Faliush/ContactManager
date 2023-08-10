using IdentityServer;
using IdentityServer.Data;
using IdentityServer.Data.Base;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var migrationAssembly = typeof(Program).Assembly.GetName().Name;

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    {
        options.UseInMemoryDatabase("MEMORY");
    })
    .AddIdentity<ApplicationUser, ApplicationRole>(config => 
    {
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequiredLength = 5;
        config.Password.RequiredUniqueChars = 3;
        config.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer(config =>
{
    config.UserInteraction.LoginUrl = "Account/Login";
})
    //.AddConfigurationStore(options =>
    //{
    //    options.ConfigureDbContext =
    //        b => b.UseSqlServer(connectionString,
    //            sql => sql.MigrationsAssembly(migrationAssembly));

    //})
    //.AddOperationalStore(options =>
    //{
    //    options.ConfigureDbContext =
    //        b => b.UseSqlServer(connectionString,
    //            sql => sql.MigrationsAssembly(migrationAssembly));
    //})
    .AddInMemoryApiResources(Configuration.GetApiResources())
    .AddInMemoryClients(Configuration.GetClients())
    .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
    .AddDeveloperSigningCredential();

builder.Services.ConfigureApplicationCookie(config => 
{
    config.LoginPath = "Account/Login";
    config.LogoutPath = "Account/Logout";
});


builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.MapControllers();

app.Run();
