using Faliush.ContactManager.Api.Definitions.Base;


var builder = WebApplication.CreateBuilder(args);

builder.AddDefinition(typeof(Program));

var app = builder.Build();

app.UseDefinition();

app.Run();

public partial class Program { }
