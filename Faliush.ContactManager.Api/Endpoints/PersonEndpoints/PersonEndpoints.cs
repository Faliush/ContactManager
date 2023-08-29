using Faliush.ContactManager.Api.Definitions.Base;
using Faliush.ContactManager.Core.Logic.PersonLogic.Queries;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using MediatR;

namespace Faliush.ContactManager.Api.Endpoints.PersonEndpoints;

public class PersonEndpoints : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        app.MapGet("api/people", GetAllPeople);
        app.MapGet("api/people/filtered", GetFilteredPeople);
        app.MapGet("api/people/{id:guid}", GetPersonById);
        app.MapGet("api/people/create", GetForCreate);
        app.MapPost("api/people", CreatePerson);
        app.MapDelete("api/people/{id:guid}", DeletePerson);
        app.MapGet("api/people/update/{id:guid}", GetPersonForUpdate);
        app.MapPut("api/people", PutAfterUpdatePerson);
        /// pagination !!
    }

    private async Task<List<PeopleViewModel>> GetAllPeople(IMediator mediator, HttpContext context) =>
        await mediator.Send(new PersonGetAllRequest(), context.RequestAborted);

    private async Task<List<PeopleViewModel>> GetFilteredPeople(
        IMediator mediator,
        HttpContext context,
        string? searchBy,
        string? searchString,
        string sortBy = "LastName",
        string sortOrder = "Asc") =>
            await mediator.Send(new PersonGetFilteredRequest(searchBy, searchString, sortBy, sortOrder), context.RequestAborted);

    private async Task<PersonViewModel> GetPersonById(Guid id, IMediator mediator, HttpContext context) =>
        await mediator.Send(new PersonGetByIdRequest(id), context.RequestAborted);

    private async Task<PersonCreateViewModel> GetForCreate(IMediator mediator, HttpContext context) =>
        await mediator.Send(new PersonGetForCreateRequest(), context.RequestAborted);

    private async Task<PersonViewModel> CreatePerson(PersonCreateViewModel viewModel, IMediator mediator, HttpContext context) =>
        await mediator.Send(new PersonCreateRequest(viewModel, context.User), context.RequestAborted);

    private async Task<Guid> DeletePerson(Guid id, IMediator mediator, HttpContext context) =>
        await mediator.Send(new PersonDeleteRequest(id), context.RequestAborted);

    private async Task<PersonUpdateViewModel> GetPersonForUpdate(Guid id, IMediator mediator, HttpContext context) =>
        await mediator.Send(new PersonGetForUpdateRequest(id), context.RequestAborted);

    private async Task<PersonViewModel> PutAfterUpdatePerson(PersonUpdateViewModel viewModel, IMediator mediator, HttpContext context) =>
        await mediator.Send(new PersonUpdateRequest(viewModel, context.User), context.RequestAborted);
}
