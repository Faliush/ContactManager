using IdentityServer.Data.DatabaseInitializer;
using IdentityServer.Definitions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.ConfigureApplicationCookie(config => 
{
    config.LoginPath = "/Account/Login";
    config.LogoutPath = "/Account/Logout";
});

builder.Services.AddControllersWithViews();

builder.Services.AddDbConnections();

var app = builder.Build();

DatabaseInitializer.SeedUser(app.Services, app.Configuration);

app.UseStaticFiles();

app.UseRouting();

app.UseCors(builder => 
{
    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});

app.UseIdentityServer();

app.MapControllers();

app.Run();
