using Faliush.ContactManager.Api.Definitions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

///
builder.AddDefinition(typeof(Program));
///

builder.Services.AddCors();

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

builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

///
app.UseDefinition();
///

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(option => option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
