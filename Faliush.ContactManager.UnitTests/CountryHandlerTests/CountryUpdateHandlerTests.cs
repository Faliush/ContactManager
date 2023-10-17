namespace Faliush.ContactManager.UnitTests.CountryHandlerTests;

public class CountryUpdateHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ILogger<CountryUpdateRequestHandler>> _loggerMock;  
    private readonly IFixture _fixture;

    public CountryUpdateHandlerTests()
    {
        _fixture = new Fixture();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _cacheServiceMock = new Mock<ICacheService>();
        _loggerMock = new Mock<ILogger<CountryUpdateRequestHandler>>();
    }

    [Fact]
    [Trait("UpdateHandleTests", nameof(Country))]
    public async Task CountryUpdateRequestHandler_Should_ThrowContactManagerArgumentException_WhenCountryNameAlreadyExists()
    {
        var country = _fixture.Build<CountryUpdateViewModel>().Create();
        var request = new CountryUpdateRequest(country);

        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, default, default, default))
             .ReturnsAsync(_fixture.Build<Country>().With(x => x.People, null as List<Person>).Create());

        var handler = new CountryUpdateRequestHandler(_unitOfWorkMock.Object, _cacheServiceMock.Object, _loggerMock.Object);

        var result = await handler.Handle(request, default);

        result.Exception.Should().BeOfType<ContactManagerArgumentException>();
    }

    [Fact]
    [Trait("UpdateHandleTests", nameof(Country))]
    public async Task CountryUpdateRequestHandler_Should_ThrowContactManagerNotFoundException_WhenCountryIdForUpdateIsNotCorrect()
    {
        var country = _fixture.Build<CountryUpdateViewModel>().Create();
        var request = new CountryUpdateRequest(country);

        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, default, default, default))
             .ReturnsAsync(null as Country);

        var handler = new CountryUpdateRequestHandler(_unitOfWorkMock.Object, _cacheServiceMock.Object, _loggerMock.Object);

        var result = await handler.Handle(request, default);

        result.Exception.Should().BeOfType<ContactManagerNotFoundException>();
    }

    [Fact]
    [Trait("UpdateHandleTests", nameof(Country))]
    public async Task CountryUpdateRequestHandler_Should_ReturnSuccessResult_WhenIdAndNameCorrect()
    {
        var country = _fixture.Build<CountryUpdateViewModel>().Create();
        var expected = new CountryViewModel() { Id = country.Id, Name = country.Name };
        var request = new CountryUpdateRequest(country);

        _unitOfWorkMock.Setup(x => x.LastSaveChangeResult).Returns(new SaveChangesResult());
        _unitOfWorkMock.SetupSequence(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, default, default, default))
            .ReturnsAsync(null as Country)
            .ReturnsAsync(_fixture.Build<Country>().With(x => x.People, null as List<Person>).With(x => x.Id, country.Id).Create());
        
        var handler = new CountryUpdateRequestHandler(_unitOfWorkMock.Object, _cacheServiceMock.Object, _loggerMock.Object);

        var result = await handler.Handle(request, default);

        result.Result.Should().BeEquivalentTo(expected);
    }
}
