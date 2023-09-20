using BlazorClient.DTO;
using BlazorClient.DTO.Results;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorClient.Pages;

public class PeopleComponentModel : ComponentBase
{
    [Inject] HttpClient HttpClient { get; set; }
    [Inject] NavigationManager NavigationManager{ get; set; }
    protected PeopleResult Result { get; set; }

    protected Dictionary<string, string> SearchFields  = new()
        {
            {"FirstName", "Name"},
            {"LastName", "Surname"},
            {"Email", "Email"}
        };
    public string SelectedSearchBy { get; set; } = "LastName";
    public string? SearchText { get; set; }
    public int IndexPage { get; set; } = 1;

    protected async Task OnPageChangedAsync(int pageNumber)
    {
        IndexPage = pageNumber;
        await ReloadData();
    }
    protected override async Task OnInitializedAsync() => await ReloadData();

    protected async Task Clear()
    {
        SelectedSearchBy = "LastName";
        SearchText = string.Empty;
        await ReloadData();
    }

    protected async Task ReloadData()
    {
        var result = await HttpClient.GetFromJsonAsync<PeopleDTO>($"people/filtered/{IndexPage - 1}?searchBy={SelectedSearchBy}&searchString={SearchText}");

        if (result is null)
            NavigationManager.NavigateTo("/error");

        if (!result!.Ok)
            NavigationManager.NavigateTo($"/error/{result.Exception["message"]}/{result.Metadata["message"]}");

        Result = result!.Result;
    }
}
