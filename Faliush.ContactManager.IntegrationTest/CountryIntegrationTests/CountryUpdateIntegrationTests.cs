using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;

namespace Faliush.ContactManager.IntegrationTests.CountryIntegrationTests;

[Collection("Test collection")]

public class CountryUpdateIntegrationTests : IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly Func<Task> _resetDatabaseAsync;
    private readonly IFixture _fixture;

    public CountryUpdateIntegrationTests(CustomWebApplicationFactory factory)
    {
        _fixture = new Fixture();
        _httpClient = factory.HttpClient;
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    [Trait("UpdateIntegrationTests", nameof(Country))]
    public async Task CountryUpdateRequest_Should_ReturnOkTrueResult_WhenInputDataIsValid()
    {
        // arrange 
        var country = _fixture.Build<CountryCreateViewModel>().Create();
        var createResponse = await _httpClient.PostAsJsonAsync("api/countries", country);
        var createdCountry = await createResponse.Content.ReadFromJsonAsync<OperationResult<CountryViewModel>>();

        var model = _fixture.Build<CountryUpdateViewModel>().With(x => x.Id, createdCountry.Result.Id).With(x => x.Name, "updatedCountry").Create();

        // act
        var response = await _httpClient.PutAsJsonAsync("api/countries", model);
        var result = await response.Content.ReadFromJsonAsync<OperationResult<CountryViewModel>>();

        // assert
        result.Should().NotBeNull();
        result!.Ok.Should().BeTrue();
    }

    public Task InitializeAsync() =>
        Task.CompletedTask;

    public async Task DisposeAsync() =>
        await _resetDatabaseAsync();
}
