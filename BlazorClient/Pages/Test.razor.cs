
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages;

public class TestModel : ComponentBase
{
	[Inject] HttpClient HttpClient { get; set; }
 	public string Result { get; set; }

	protected override async Task OnInitializedAsync()
	{
		Result = await HttpClient.GetStringAsync("test/secret");
	}
}
