using BlazorClient.DTO;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorClient.Pages.Countries;

public class CountriesComponentModel : ComponentBase
{
    [Inject] HttpClient HttpClient { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    protected CountriesDTO? Response { get; set; }

    protected override async Task OnInitializedAsync() => await ReloadData();

    private async Task ReloadData()
    {
        var result = await HttpClient.GetFromJsonAsync<CountriesDTO>("countries");

        if (result is null)
            NavigationManager.NavigateTo("/error");

        if (!result!.Ok)
            NavigationManager.NavigateTo($"/error/{result.Exception["message"]}/{result.Metadata["message"]}");

        Response = result;
    }
}
