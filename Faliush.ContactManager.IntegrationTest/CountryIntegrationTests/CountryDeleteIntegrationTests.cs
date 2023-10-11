using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;

namespace Faliush.ContactManager.IntegrationTests.CountryIntegrationTests;

[Collection("Test collection")]

public class CountryDeleteIntegrationTests : IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly Func<Task> _resetDatabaseAsync;
    private readonly IFixture _fixture;

    public CountryDeleteIntegrationTests(CustomWebApplicationFactory factory)
    {
        _fixture = new Fixture();
        _httpClient = factory.HttpClient;
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    [Trait("DeleteIntegrationTests", nameof(Country))]
    public async Task CountryDeleteRequest_Should_ReturnOkTrueResult_WhenInputDataIsValid()
    {
        var model = _fixture.Build<CountryCreateViewModel>().Create();
        var createResponse = await _httpClient.PostAsJsonAsync("api/countries", model);
        var country = await createResponse.Content.ReadFromJsonAsync<OperationResult<CountryViewModel>>();

        var response = await _httpClient.DeleteAsync($"api/countries/{country.Result.Id}");
        var result = await response.Content.ReadFromJsonAsync<OperationResult<Guid>>();

        result.Should().NotBeNull();
        result!.Ok.Should().BeTrue();
    }

    public Task InitializeAsync() =>
        Task.CompletedTask;

    public async Task DisposeAsync() =>
        await _resetDatabaseAsync();
}
