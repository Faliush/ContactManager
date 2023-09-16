using BlazorBootstrap;
using BlazorClient.DTO;
using BlazorClient.DTO.Results;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorClient.Components;

public class PersonDetailComponentModel : ComponentBase
{
    [Inject] HttpClient HttpClient { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    protected PersonResult Result { get; set; }

    [Parameter] public Guid PersonId { get; set; }
    protected Modal? modal = default!;

    protected async Task ShowPersonDetailComponent()
    {
        var result = await HttpClient.GetFromJsonAsync<PersonDTO>($"people/{PersonId}");

        if (result is null)
            NavigationManager.NavigateTo("/error");

        if (!result!.Ok)
            NavigationManager.NavigateTo($"/error/{result.Exception!.ContainsKey("message")}/{result.Metadata!.ContainsKey("message")}");

        Result = result!.Result;

        await modal.ShowAsync();
    }
}
