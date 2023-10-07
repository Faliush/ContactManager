using BlazorBootstrap;
using BlazorClient.DTO;
using BlazorClient.DTO.Results;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorClient.Pages.People;

public class UpdatePersonComponentModel : ComponentBase
{
    [Inject] HttpClient HttpClient { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] ToastService ToastService { get; set; } = default!;
    [Parameter] public Guid PersonId { get; set; }

    protected UpdatePersonResult? PersonResult { get; set; }
    protected CountriesDTO? CountriesDTO { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var personResult = await HttpClient.GetFromJsonAsync<UpdatePersonDTO>($"people/update/{PersonId}");
        var listCountries = await HttpClient.GetFromJsonAsync<CountriesDTO>("countries");

        if (personResult is null || listCountries is null)
            NavigationManager.NavigateTo("/error");

        if (!personResult!.Ok)
            NavigationManager.NavigateTo($"/error/{personResult.Exception["message"]}/{personResult.Metadata["message"]}");

        if (!listCountries!.Ok)
            NavigationManager.NavigateTo($"/error/{listCountries.Exception["message"]}/{listCountries.Metadata["message"]}");

        PersonResult = personResult.Result;
        CountriesDTO = listCountries;
    }

    protected async Task ValidSubmit()
    {
        var repsponse = await HttpClient.PutAsJsonAsync("people", PersonResult);
        var result = await repsponse.Content.ReadFromJsonAsync<PersonDTO>();

        if (!result.Ok)
            ToastService.Notify(new ToastMessage(ToastType.Warning, $"{result.Metadata["message"]}", $"{result.Exception["message"]}"));

        //ToastService.Notify(new ToastMessage(ToastType.Success, $"Person was updated successfully.")); // TODO: 
        NavigationManager.NavigateTo("/people");
    }

}
