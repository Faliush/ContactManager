using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;

namespace Faliush.ContactManager.IntegrationTests.PersonIntegrationTests;

[Collection("Test collection")]
public class PersonCreateIntegrationTests : IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly Func<Task> _resetDatabaseAsync;
    private readonly IFixture _fixture;

    public PersonCreateIntegrationTests(CustomWebApplicationFactory factory)
    {
        _fixture = new Fixture();
        _httpClient = factory.HttpClient;
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    [Trait("CreateIntegrationTests", nameof(Person))]
    public async Task PersonCreateRequest_Should_ReturnOkTrueResult_WhenInputDataIsValid()
    {
        var country = _fixture.Build<CountryCreateViewModel>().Create();
        var countryResponse = await _httpClient.PostAsJsonAsync("api/countries", country, default);
        var countryResult = await countryResponse.Content.ReadFromJsonAsync<OperationResult<CountryViewModel>>();
        
        var model = _fixture.Build<PersonCreateViewModel>()
            .With(x => x.FirstName, "Create")
            .With(x => x.LastName, "Create")
            .With(x => x.Email, "createemail@gmail.com")
            .With(x => x.DateOfBirth, DateTime.Parse("08.12.2004"))
            .With(x => x.CountryId, countryResult.Result.Id)
            .Create();

        var response = await _httpClient.PostAsJsonAsync("api/people", model, default);
        var result = await response.Content.ReadFromJsonAsync<OperationResult<PersonViewModel>>();

        result.Should().NotBeNull();
        result!.Ok.Should().BeTrue();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabaseAsync();
}
