using Faliush.ContactManager.Api.Definitions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Faliush.ContactManager.Api.Definitions.Identity;

public class AuthorizationDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(config =>
            {
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromSeconds(5),
                    ValidateAudience = false
                };
                config.Authority = "https://localhost:10001";
                config.Audience = "https://localhost:10001";
            });

        builder.Services.AddAuthorization(config => 
        {
            config.AddPolicy("Administrator", builder =>
            {
                builder.RequireClaim(ClaimTypes.Role, "Administrator");
            });
        }); ;
    }

    public override void ConfigureApplication(WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
