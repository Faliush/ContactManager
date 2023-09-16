using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages;

public class ErrorComponent : ComponentBase
{
	[Parameter] public string? MetaMessage { get; set; }
	[Parameter] public string? ExceptionMessage { get; set; }

}
