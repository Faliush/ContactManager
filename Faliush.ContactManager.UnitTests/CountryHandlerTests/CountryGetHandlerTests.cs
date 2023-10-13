namespace Faliush.ContactManager.UnitTests.CountryHandlerTests;

public class CountryGetHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CountryGetAllRequestHandler>> _loggerGetAllMock;
    private readonly Mock<ILogger<CountryGetForUpdateRequestHandler>> _loggerGetForUpdateMock;
    private readonly IFixture _fixture;
    private readonly IMapper _mapper;

    public CountryGetHandlerTests()
    {
        _fixture = new Fixture();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerGetAllMock = new Mock<ILogger<CountryGetAllRequestHandler>>();
        _loggerGetForUpdateMock = new Mock<ILogger<CountryGetForUpdateRequestHandler>>();
        
        if (_mapper == null)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CountryMapperConfiguration());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }
    }

    [Fact]
    [Trait("GetHandleTests", nameof(Country))]
    public async Task CountryGetAllRequestHandler_Should_ReturnEmptyResult_WhenCountriesDoesNotExistInDatabase()
    {
        //arrange
        var request = new CountryGetAllRequest();

        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetAllAsync(default, default, default, default, default, default))
            .ReturnsAsync(new List<Country>());

        var handler = new CountryGetAllRequestHandler(_unitOfWorkMock.Object, _mapper, _loggerGetAllMock.Object);

        // act 

        var result = await handler.Handle(request, default);

        // assert 
        result.Result.Should().BeEmpty();
    }

    [Fact]
    [Trait("GetHandleTests", nameof(Country))]
    public async Task CountryGetAllRequestHandler_Should_ReturnSomeCountries_WhenDatabaseIsNotEmpty()
    {
        // arrange
        var request = new CountryGetAllRequest();

        var countries = new List<Country>()
        {
            _fixture.Build<Country>().With(x => x.People, null as List<Person>).Create(),
            _fixture.Build<Country>().With(x => x.People, null as List<Person>).Create(),
            _fixture.Build<Country>().With(x => x.People, null as List<Person>).Create()
        };
        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetAllAsync(default, default, default, default, default, default))
            .ReturnsAsync(countries);

        var expected = countries.Select(x => new CountryViewModel() { Name = x.Name, Id = x.Id }).ToList();

        var handler = new CountryGetAllRequestHandler(_unitOfWorkMock.Object, _mapper, _loggerGetAllMock.Object);

        // act 
        var result = await handler.Handle(request, default);

        // assert 
        result.Result.Should().NotBeEmpty();
        result.Result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    [Trait("GetHandleTests", nameof(Country))]
    public async Task CountryGetForUpdateRequestHandler_Should_ThrowContactManagerNotFoundException_WhenCountryIdDoesNotExist()
    {
        var countryId = Guid.Parse("1932C678-E513-4349-8458-77EADA874B94");
        var request = new CountryGetForUpdateRequest(countryId);

        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, default, default, default))
             .ReturnsAsync(null as Country);

        var handler = new CountryGetForUpdateRequestHandler(_unitOfWorkMock.Object, _loggerGetForUpdateMock.Object);

        var result = await handler.Handle(request, default);

        result.Exception.Should().BeOfType<ContactManagerNotFoundException>();
    }

    [Fact]
    [Trait("GetHandleTests", nameof(Country))]
    public async Task CountryGetForUpdateRequestHandler_Should_ReturnSuccessResult_WhenIdIsCorrect()
    {
        var country = _fixture.Build<Country>().With(x => x.People, null as List<Person>).Create();
        var expected = new CountryUpdateViewModel() { Id = country.Id, Name = country.Name };
        var request = new CountryGetForUpdateRequest(country.Id);

        _unitOfWorkMock.Setup(x => x.LastSaveChangeResult).Returns(new SaveChangesResult());
        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, true, default, default))
             .ReturnsAsync(country);

        var handler = new CountryGetForUpdateRequestHandler(_unitOfWorkMock.Object, _loggerGetForUpdateMock.Object);

        var result = await handler.Handle(request, default);

        result.Result.Should().BeEquivalentTo(expected);
    }
}
