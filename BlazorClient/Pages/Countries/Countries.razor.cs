using BlazorBootstrap;
using BlazorClient.DTO;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorClient.Pages.Countries;

public class CountriesComponentModel : ComponentBase
{
    [Inject] HttpClient HttpClient { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] ToastService ToastService { get; set; }
    protected CountriesDTO? Response { get; set; }

    protected ConfirmDialog? dialog = default!;

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

    protected async Task ShowConfirmationAsync(Guid Id)
    {
        var confirmation = await dialog.ShowAsync(
            title: "Are you sure you want to delete this?",
            message1: "This will delete the record. Once deleted can not be rolled back.",
            message2: "Do you want to proceed?");

        if (confirmation)
        {
            var response = await HttpClient.DeleteAsync($"countries/{Id}");
            var result = await response.Content.ReadFromJsonAsync<DeleteDTO>();
            
            if (!result.Ok)
                ToastService.Notify(new ToastMessage(ToastType.Warning, $"{result.Metadata["message"]}", $"{result.Exception["message"]}"));

            ToastService.Notify(new ToastMessage(ToastType.Success, $"Person deleted successfully."));
            await ReloadData();
        }
        else
        {
            ToastService.Notify(new ToastMessage(ToastType.Secondary, $"Delete action canceled."));
        }
    }
}
