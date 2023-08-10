using IdentityServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var migrationAssembly = typeof(Program).Assembly.GetName().Name;

builder.Services.AddIdentityServer()
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
