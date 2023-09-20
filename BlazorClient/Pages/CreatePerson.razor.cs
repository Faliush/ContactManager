using BlazorBootstrap;
using BlazorClient.DTO;
using BlazorClient.DTO.Results;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorClient.Pages;

public class CreatePersonComponentModel : ComponentBase
{
	[Inject] HttpClient HttpClient { get; set; }
	[Inject] NavigationManager NavigationManager { get; set; }

	protected GetForCreatePersonResult? Result { get; set; }
	protected CreatePersonDTO Model { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		var result = await HttpClient.GetFromJsonAsync<GetForCreatePersonDTO>("people/create");

		if (result is null)
			NavigationManager.NavigateTo("/error");

		if (!result!.Ok)
			NavigationManager.NavigateTo($"/error/{result.Exception!.ContainsKey("message")}/{result.Metadata!.ContainsKey("message")}");

		Result = result!.Result;
	}

	protected async Task ValidSubmit()
	{
		var response = await HttpClient.PostAsJsonAsync("people", Model);

		var result = await response.Content.ReadFromJsonAsync<PersonDTO>();

		if (!result!.Ok)
		{
            NavigationManager.NavigateTo($"/error/{result.Exception!.ContainsKey("message")}/{result.Metadata!.ContainsKey("message")}");
			ShowMessage(ToastType.Danger, message: "Something went wrong");
		}

		ShowMessage(ToastType.Success, message: "The person was created successfully");
		Model = new();
    }

    protected List<ToastMessage> messages = new List<ToastMessage>();

    protected void ShowMessage(ToastType toastType, string message) => messages.Add(CreateToastMessage(toastType, message));

    protected ToastMessage CreateToastMessage(ToastType toastType, string message)
    => new ToastMessage
    {
        Type = toastType,
        Title = "Contact Manager",
        HelpText = DateTime.Now.ToString("d"),
        Message = message
    };

}
