namespace Faliush.ContactManager.UnitTests.CountryHandlerTests;

public class CountryDeleteHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IFixture _fixture;

    public CountryDeleteHandlerTests()
    {
        _fixture = new Fixture();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    [Trait("DeleteHandleTests", nameof(Country))]
    public async Task Handler_Should_ThrowContactManagerNotFoundException_WhenCountryIdIsNotExistForDelete()
    {
        var countryId = Guid.Parse("1932C678-E513-4349-8458-77EADA874B94");
        var request = new CountryDeleteRequest(countryId);

        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, default, default, default))
             .ReturnsAsync(null as Country);

        var handler = new CountryDeleteRequestHandler(_unitOfWorkMock.Object);

        var result = await handler.Handle(request, default);

        result.Exception.Should().BeOfType<ContactManagerNotFoundException>();
    }

    [Fact]
    [Trait("DeleteHandleTests", nameof(Country))]
    public async Task Handler_Should_ReturnSuccessResult_WhenIdIsCorrectForDelete()
    {
        var country = _fixture.Build<Country>().With(x => x.People, null as List<Person>).Create();
        var request = new CountryDeleteRequest(country.Id);

        _unitOfWorkMock.Setup(x => x.LastSaveChangeResult).Returns(new SaveChangesResult());
        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, default, default, default))
             .ReturnsAsync(country);

        var handler = new CountryDeleteRequestHandler(_unitOfWorkMock.Object);

        var result = await handler.Handle(request, default);

        result.Result.Should().Be(country.Id);
    }
}