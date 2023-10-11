using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;

namespace Faliush.ContactManager.IntegrationTests.PersonIntegrationTests;

[Collection("Test collection")]
public class PersonUpdateIntegrationTests : IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly Func<Task> _resetDatabaseAsync;
    private readonly IFixture _fixture;

    public PersonUpdateIntegrationTests(CustomWebApplicationFactory factory)
    {
        _fixture = new Fixture();
        _httpClient = factory.HttpClient;
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    [Trait("UpdateIntegrationTests", nameof(Person))]
    public async Task UpdatePersonRequest_Should_ReturnOkTrueResult_WhenInputDataIsValid()
    {
        var country = _fixture.Build<CountryCreateViewModel>().Create();
        var countryResponse = await _httpClient.PostAsJsonAsync("api/countries", country, default);
        var countryResult = await countryResponse.Content.ReadFromJsonAsync<OperationResult<CountryViewModel>>();

        var createModel = _fixture.Build<PersonCreateViewModel>()
            .With(x => x.FirstName, "Update")
            .With(x => x.LastName, "Update")
            .With(x => x.Email, "updateemail@gmail.com")
            .With(x => x.DateOfBirth, DateTime.Parse("08.12.2004"))
            .With(x => x.CountryId, countryResult.Result.Id)
            .Create();

        var createResponse = await _httpClient.PostAsJsonAsync("api/people", createModel, default);
        var person = await createResponse.Content.ReadFromJsonAsync<OperationResult<PersonViewModel>>();

        var model = _fixture.Build<PersonUpdateViewModel>()
            .With(x => x.Id, person.Result.Id)
            .With(x => x.FirstName, "name")
            .With(x => x.LastName, "surname")
            .With(x => x.Email, "bababubu@gmail.com")
            .With(x => x.DateOfBirth, DateTime.Parse("08.12.2004"))
            .With(x => x.CountryId, countryResult.Result.Id)
            .Create();
        var response = await _httpClient.PutAsJsonAsync("api/people/", model);
        var result = await response.Content.ReadFromJsonAsync<OperationResult<PersonViewModel>>();

        result.Should().NotBeNull();
        result!.Ok.Should().BeTrue();
    }


    public Task InitializeAsync() =>
        Task.CompletedTask;

    public async Task DisposeAsync() =>
        await _resetDatabaseAsync();
}
