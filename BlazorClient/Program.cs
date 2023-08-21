using BlazorClient;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Security.Claims;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
builder.Services.AddHttpClient("ContactAPI", cl =>
{
    cl.BaseAddress = new Uri("https://localhost:5001/api/");
})
.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
builder.Services.AddScoped(
   sp => sp.GetService<IHttpClientFactory>().CreateClient("ContactAPI"));

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.ClientId = "blazor_client";
    options.ProviderOptions.Authority = "https://localhost:10001";

    options.ProviderOptions.ResponseType = "code";

    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("ContactAPI");

    options.UserOptions.NameClaim = ClaimTypes.Name;

}).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("AdministratorPage", builder =>
    {
        builder.RequireClaim(ClaimTypes.Role, "Administrator");
    });
});

await builder.Build().RunAsync();
