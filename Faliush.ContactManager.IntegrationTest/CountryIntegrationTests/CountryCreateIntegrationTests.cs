using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;

namespace Faliush.ContactManager.IntegrationTests.CountryIntegrationTests;

[Collection("Test collection")]

public class CountryCreateIntegrationTests : IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly Func<Task> _resetDatabaseAsync;
    private readonly IFixture _fixture;

    public CountryCreateIntegrationTests(CustomWebApplicationFactory factory)
    {
        _fixture = new Fixture();   
        _httpClient = factory.HttpClient;
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    [Trait("CreateIntegrationTests", nameof(Country))]
    public async Task CountryCreateRequest_Should_ReturnOkTrueResult_WhenInputDataIsValid()
    {
        var model = _fixture.Build<CountryCreateViewModel>().With(x => x.Name, "country").Create();

        var response = await _httpClient.PostAsJsonAsync("api/countries", model, default);
        var result = await response.Content.ReadFromJsonAsync<OperationResult<CountryViewModel>>();

        result.Should().NotBeNull();
        result!.Ok.Should().BeTrue();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabaseAsync();

}
