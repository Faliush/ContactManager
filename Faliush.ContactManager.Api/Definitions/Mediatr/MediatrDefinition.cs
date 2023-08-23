using Faliush.ContactManager.Api.Definitions.Base;
using Faliush.ContactManager.Core.Common.Mediatr;
using Faliush.ContactManager.Core.Common.ValidationBehavior;
using MediatR;

namespace Faliush.ContactManager.Api.Definitions.Mediatr;

public class MediatrDefinition : AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        builder.Services.AddMeditorCore();
    }
}
