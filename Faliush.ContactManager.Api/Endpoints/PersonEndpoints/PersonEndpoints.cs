using Faliush.ContactManager.Api.Definitions.Base;
using Faliush.ContactManager.Core.Common.OperationResult;
using Faliush.ContactManager.Core.Logic.PersonLogic.Queries;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Faliush.ContactManager.Api.Endpoints.PersonEndpoints;

public class PersonEndpoints : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        app.MapGet("api/people/filtered", GetFilteredPeople);
        app.MapGet("api/people/{id:guid}", GetPersonById);
        app.MapPost("api/people", CreatePerson);
        app.MapDelete("api/people/{id:guid}", DeletePerson);
        app.MapGet("api/people/update/{id:guid}", GetPersonForUpdate);
        app.MapPut("api/people", PutAfterUpdatePerson);
        app.MapGet("api/people/filtered/{pageIndex:int}", GetFilteredPagedPeople);
    }

    private async Task<OperationResult<List<PeopleViewModel>>> GetFilteredPeople(
        IMediator mediator,
        HttpContext context,
        string? searchBy,
        string? searchString,
        string sortBy = "LastName",
        string sortOrder = "Asc") =>
            await mediator.Send(new PersonGetFilteredRequest(searchBy, searchString, sortBy, sortOrder), context.RequestAborted);

    private async Task<OperationResult<IPagedList<PeopleViewModel>>> GetFilteredPagedPeople(
        IMediator mediator,
        IConfiguration configuration,
        HttpContext context,
        int pageIndex,
        string? searchBy,
        string? searchString,
        string sortBy = "LastName",
        string sortOrder = "Asc") =>
            await mediator.Send(new PersonGetFilteredPagedRequest(
                pageIndex, 
                configuration.GetValue<int>("PageSize"), 
                searchBy, 
                searchString, 
                sortBy, 
                sortOrder), context.RequestAborted);
    
    [Authorize]
    private async Task<OperationResult<PersonViewModel>> GetPersonById(Guid id, IMediator mediator, HttpContext context) =>
        await mediator.Send(new PersonGetByIdRequest(id), context.RequestAborted);


    [Authorize]
    private async Task<OperationResult<PersonViewModel>> CreatePerson(PersonCreateViewModel viewModel, IMediator mediator, HttpContext context) =>
        await mediator.Send(new PersonCreateRequest(viewModel, context.User), context.RequestAborted);

    [Authorize]
    private async Task<OperationResult<Guid>> DeletePerson(Guid id, IMediator mediator, HttpContext context) =>
        await mediator.Send(new PersonDeleteRequest(id), context.RequestAborted);

    [Authorize]
    private async Task<OperationResult<PersonUpdateViewModel>> GetPersonForUpdate(Guid id, IMediator mediator, HttpContext context) =>
        await mediator.Send(new PersonGetForUpdateRequest(id), context.RequestAborted);

    [Authorize]
    private async Task<OperationResult<PersonViewModel>> PutAfterUpdatePerson(PersonUpdateViewModel viewModel, IMediator mediator, HttpContext context) =>
        await mediator.Send(new PersonUpdateRequest(viewModel, context.User), context.RequestAborted);
}
