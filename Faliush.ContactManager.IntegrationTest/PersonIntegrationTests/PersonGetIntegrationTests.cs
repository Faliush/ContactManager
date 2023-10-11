using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;

namespace Faliush.ContactManager.IntegrationTests.PersonIntegrationTests;

[Collection("Test collection")]
public class PersonGetIntegrationTests : IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly Func<Task> _resetDatabaseAsync;
    private readonly IFixture _fixture;

    public PersonGetIntegrationTests(CustomWebApplicationFactory factory)
    {
        _fixture = new Fixture();
        _httpClient = factory.HttpClient;
        _resetDatabaseAsync = factory.ResetDatabaseAsync;
    }

    [Fact]
    [Trait("GetIntegrationTests", nameof(Person))]
    public async Task PersonGetFilteredRequest_Should_ReturnOkTrueResult_WhenInputDataIsValid()
    {
        var response = await _httpClient.GetAsync("api/people/filtered");
        var result = await response.Content.ReadFromJsonAsync<OperationResult<List<PeopleViewModel>>>();

        result.Should().NotBeNull();
        result.Ok.Should().BeTrue();
    }

    //[Fact]
    //[Trait("GetIntegrationTests", nameof(Person))]
    //public async Task PersonGetFilteredPagedRequest_Should_ReturnOkTrueResult_WhenInputDataIsValid()
    //{
    //    var page = 0;

    //    var response = await _httpClient.GetAsync($"api/people/filtered/{page}");
    //    var result = await response.Content.ReadFromJsonAsync<OperationResult<PagedList<PeopleViewModel>>>();

    //    result.Should().NotBeNull();
    //    result.Ok.Should().BeTrue();
    //}


    public Task InitializeAsync() =>
        Task.CompletedTask;

    public async Task DisposeAsync() =>
        await _resetDatabaseAsync();
}
