using Faliush.ContactManager.Api.Definitions.Base;
using Faliush.ContactManager.Core;
using Faliush.ContactManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.AddDefinition(typeof(Program));

var app = builder.Build();

app.UseDefinition();

app.Run();
