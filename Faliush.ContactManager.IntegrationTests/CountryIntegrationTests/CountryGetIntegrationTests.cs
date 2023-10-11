using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;

namespace Faliush.ContactManager.IntegrationTests.CountryIntegrationTests;

[Collection("Test collection")]

public class CountryGetIntegrationTests : IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly Func<Task> _resetDatabaseAsync;
    private readonly IFixture _fixture;
    public CountryGetIntegrationTests(CustomWebApplicationFactory factory)
    {
        _fixture = new Fixture();
        _httpClient = factory.HttpClient;
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    [Trait("GetIntegrationTests", nameof(Country))]
    public async Task CountryGetAllRequest_Should_ReturnOkTrueResult_WhenDatabaseIsNotEmpty()
    {

        var response = await _httpClient.GetAsync("api/countries");
        var result = await response.Content.ReadFromJsonAsync<OperationResult<List<CountryViewModel>>>();

        result.Should().NotBeNull();
        result.Ok.Should().BeTrue();
    }

    [Fact]
    [Trait("GetIntegrationTests", nameof(Country))]
    public async Task CountryGetForUpdate_Should_ReturnOkTrueResult_WhenIdExists()
    {
        var model = _fixture.Build<CountryCreateViewModel>().Create();
        var createResponse = await _httpClient.PostAsJsonAsync("api/countries", model);
        var country = await createResponse.Content.ReadFromJsonAsync<OperationResult<CountryViewModel>>();

        var response = await _httpClient.GetAsync($"api/countries/update/{country.Result.Id}");
        var result = await response.Content.ReadFromJsonAsync<OperationResult<CountryUpdateViewModel>>();

        result.Should().NotBeNull();
        result!.Ok.Should().BeTrue();
    }

    public Task InitializeAsync() =>
        Task.CompletedTask;

    public async Task DisposeAsync() =>
        await _resetDatabaseAsync();
}
