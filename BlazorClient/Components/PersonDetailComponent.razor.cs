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
    [Inject] ToastService ToastService { get; set; } = default!;
    protected PersonResult Result { get; set; }
    [Parameter] public Guid PersonId { get; set; }
    [Parameter] public EventCallback ReloadData { get; set; }
    
    protected Modal? modal = default!;
    protected ConfirmDialog? dialog = default!;

    protected async Task ShowPersonDetailComponent()
    {
        var result = await HttpClient.GetFromJsonAsync<PersonDTO>($"people/{PersonId}");

        if (result is null)
            NavigationManager.NavigateTo("/error");

        if (!result!.Ok)
            NavigationManager.NavigateTo($"/error/{result.Exception["message"]}/{result.Metadata["message"]}");
        

        Result = result!.Result;

        await modal.ShowAsync();
    }


    protected async Task ShowConfirmationAsync()
    {
        var confirmation = await dialog.ShowAsync(
            title: "Are you sure you want to delete this?",
            message1: "This will delete the record. Once deleted can not be rolled back.",
            message2: "Do you want to proceed?");

        if (confirmation)
        {
            var response = await HttpClient.DeleteAsync($"people/{PersonId}");
            var result = await response.Content.ReadFromJsonAsync<DeleteDTO>();
            
            if (!result.Ok)
                ToastService.Notify(new ToastMessage(ToastType.Warning, $"{result.Metadata["message"]}", $"{result.Exception["message"]}"));
            

            ToastService.Notify(new ToastMessage(ToastType.Success, $"Employee deleted successfully."));
            await modal.HideAsync();
            await NeedReloadData();
        }
        else
        {
            ToastService.Notify(new ToastMessage(ToastType.Secondary, $"Delete action canceled."));
        }
    }

    protected async Task NeedReloadData()
    {
        await ReloadData.InvokeAsync();
    }
}
