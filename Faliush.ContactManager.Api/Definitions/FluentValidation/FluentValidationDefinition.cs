using Faliush.ContactManager.Api.Definitions.Base;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Faliush.ContactManager.Api.Definitions.FluentValidation;

public class FluentValidationDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.Configure<ApiBehaviorOptions>(option =>
        {
            option.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
    }
}
