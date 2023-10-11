using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;

namespace Faliush.ContactManager.IntegrationTests.PersonIntegrationTests;

[Collection("Test collection")]
public class PersonDeleteIntegrationTests : IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly Func<Task> _resetDatabaseAsync;
    private readonly IFixture _fixture;

    public PersonDeleteIntegrationTests(CustomWebApplicationFactory factory)
    {
        _fixture = new Fixture();
        _httpClient = factory.HttpClient;
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    [Trait("DeleteIntegrationTests", nameof(Person))]
    public async Task PersonDeleteRequest_Should_ReturnOkTrueResult_WhenInputDataIsValid()
    {
        var country = _fixture.Build<CountryCreateViewModel>().Create();
        var countryResponse = await _httpClient.PostAsJsonAsync("api/countries", country, default);
        var countryResult = await countryResponse.Content.ReadFromJsonAsync<OperationResult<CountryViewModel>>();
        
        var model = _fixture.Build<PersonCreateViewModel>()
           .With(x => x.FirstName, "Delete")
           .With(x => x.LastName, "Delete")
           .With(x => x.Email, "deleteemail1@gmail.com")
           .With(x => x.DateOfBirth, DateTime.Parse("08.11.2004"))
           .With(x => x.CountryId, countryResult.Result.Id)
           .Create();
        var createResponse = await _httpClient.PostAsJsonAsync("api/people", model);
        var person = await createResponse.Content.ReadFromJsonAsync<OperationResult<PersonViewModel>>();

        var response = await _httpClient.DeleteAsync($"api/people/{person.Result.Id}");
        var result = await response.Content.ReadFromJsonAsync<OperationResult<Guid>>();

        result.Should().NotBeNull();    
        result.Ok.Should().BeTrue();
    }


    public Task InitializeAsync() =>
         Task.CompletedTask;

    public async Task DisposeAsync() =>
        await _resetDatabaseAsync();
}
