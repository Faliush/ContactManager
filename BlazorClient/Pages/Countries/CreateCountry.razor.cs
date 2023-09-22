using BlazorBootstrap;
using BlazorClient.DTO;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorClient.Pages.Countries;

public class CreateCountryComponentModel : ComponentBase
{
	[Inject] HttpClient HttpClient { get; set; }
	[Inject] NavigationManager NavigationManager { get; set; }

	protected CreateCountryDTO Model { get; set; } = new();

	protected async Task ValidSubmit()
	{
		var response = await HttpClient.PostAsJsonAsync("countries", Model);
		var result = await response.Content.ReadFromJsonAsync<CountryDTO>();

		if (!result!.Ok)
		{
			ShowMessage(ToastType.Danger, message: "Something went wrong");
			NavigationManager.NavigateTo($"/error/{result.Exception["message"]}/{result.Metadata["message"]}");
		}

		ShowMessage(ToastType.Success, message: "The country was created successfully");
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
