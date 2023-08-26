using Faliush.ContactManager.Api.Definitions.Base;

namespace Faliush.ContactManager.Api.Endpoints.PersonEndpoints;

public class PersonEndpoints : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        app.MapGet("api/people", GetAllPeople);
        app.MapGet("api/people/filtered", GetFilteredPeople);
        app.MapGet("api/people/{id:guid}", GetPersonById);
        app.MapPost("api/people", CreatePerson);
        app.MapDelete("api/people", DeletePerson);
        app.MapGet("api/people/update/{id:guid}", GetPersonForUpdate);
        app.MapPut("api/people", PutAfterUpdatePerson);
    }
}
