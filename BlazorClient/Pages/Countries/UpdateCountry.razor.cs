using BlazorBootstrap;
using BlazorClient.DTO;
using BlazorClient.DTO.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorClient.Pages.Countries;

public class UpdateCountryComponentModel : ComponentBase
{
	[Inject] HttpClient HttpClient { get; set; }
	[Inject] NavigationManager NavigationManager { get; set; }
	[Inject] ToastService ToastService { get; set; }
	[Parameter] public Guid CountryId { get; set; }

	protected CountryModel? Model { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		var result = await HttpClient.GetFromJsonAsync<CountryDTO>($"countries/update/{CountryId}");

		if (result is null)
			NavigationManager.NavigateTo("/error");

		if (!result!.Ok)
			NavigationManager.NavigateTo($"/error/{result.Exception["message"]}/{result.Metadata["message"]}");

		Model = result.Result;
	}

	protected async Task ValidSubmit()
	{
		var repsponse = await HttpClient.PutAsJsonAsync("countries", Model);
		var result = await repsponse.Content.ReadFromJsonAsync<CountryDTO>();

		if (!result.Ok)
			ToastService.Notify(new ToastMessage(ToastType.Warning, $"{result.Metadata["message"]}", $"{result.Exception["message"]}"));

		//ToastService.Notify(new ToastMessage(ToastType.Success, $"Person was updated successfully.")); // TODO: 
		NavigationManager.NavigateTo("/countries");
	}


}
