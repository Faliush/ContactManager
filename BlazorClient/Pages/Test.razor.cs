
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages;

public class TestModel : ComponentBase
{
	//[Inject] TestHttpClient TestHttpClient { get; set; }
	[Inject] HttpClient HttpClient { get; set; }
 	public string Result { get; set; }

	protected override async Task OnInitializedAsync()
	{
		//Result = await TestHttpClient.GetSecretAsync();
		Result = await HttpClient.GetStringAsync("test/secret");
	}
}
