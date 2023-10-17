using System.Security.Claims;

namespace Faliush.ContactManager.UnitTests.PersonHandlerTests;

public class PersonUpdateHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDateCalcualtorService> _dateCalculatorMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ILogger<PersonUpdateRequestHandler>> _loggerMock;
    private readonly ClaimsPrincipal _user;
    private readonly IFixture _fixture;
    private readonly IMapper _mapper;

    public PersonUpdateHandlerTests()
    {
        _fixture = new Fixture();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dateCalculatorMock = new Mock<IDateCalcualtorService>();
        _cacheServiceMock = new Mock<ICacheService>();  
        _loggerMock = new Mock<ILogger<PersonUpdateRequestHandler>>();
        _user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, "mock") }, "mock"));

        if (_mapper == null)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PersonMapperConfiguration());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }
    }

    [Fact]
    [Trait("UpdateHandleTests", nameof(Person))]
    public async Task PersonUpdateRequestHandler_Should_ThrowContactManagerArgumentException_WhenrRenewablePersonNameAlreadyExists()
    {
        var personUpdateViewModel = _fixture.Build<PersonUpdateViewModel>().Create();
        var request = new PersonUpdateRequest(personUpdateViewModel, _user);

        _unitOfWorkMock.Setup(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, false, default, default))
            .ReturnsAsync(_fixture.Build<Person>().With(x => x.Country, null as Country).Create());

        var handler = new PersonUpdateRequestHandler(_unitOfWorkMock.Object, _mapper, _dateCalculatorMock.Object, _cacheServiceMock.Object, _loggerMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeFalse();
        result.Exception.Should().BeOfType<ContactManagerArgumentException>();
    }

    [Fact]
    [Trait("UpdateHandleTests", nameof(Person))]
    public async Task PersonUpdateRequestHandler_Should_ThrowContactManagerNotFoundException_WhenrRenewablePersonIdIsNotExist()
    {
        var personUpdateViewModel = _fixture.Build<PersonUpdateViewModel>().Create();
        var request = new PersonUpdateRequest(personUpdateViewModel, _user);

        _unitOfWorkMock.Setup(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, false, default, default))
            .ReturnsAsync(null as Person);

        var handler = new PersonUpdateRequestHandler(_unitOfWorkMock.Object, _mapper, _dateCalculatorMock.Object, _cacheServiceMock.Object, _loggerMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeFalse();
        result.Exception.Should().BeOfType<ContactManagerNotFoundException>();
    }

    [Fact]
    [Trait("UpdateHandleTests", nameof(Person))]
    public async Task PersonUpdateRequestHandler_Should_ThrowContactManagerNotFoundException_WhenCountryIsNotExist()
    {
        var personUpdateViewModel = _fixture.Build<PersonUpdateViewModel>().Create();
        var request = new PersonUpdateRequest(personUpdateViewModel, _user);

        _unitOfWorkMock.SetupSequence(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, false, default, default))
            .ReturnsAsync(null as Person)
            .ReturnsAsync(_fixture.Build<Person>().With(x => x.Country, null as Country).Create());
        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, false, default, default))
            .ReturnsAsync(null as Country);

        var handler = new PersonUpdateRequestHandler(_unitOfWorkMock.Object, _mapper, _dateCalculatorMock.Object, _cacheServiceMock.Object, _loggerMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeFalse();
        result.Exception.Should().BeOfType<ContactManagerNotFoundException>();
    }

    [Fact]
    [Trait("UpdateHandleTests", nameof(Person))]
    public async Task PersonUpdateRequestHandler_Should_ThrowContactManagerSaveDatabaseException_WhenSaveChangeResultIsNotOk()
    {
        var personUpdateViewModel = _fixture.Build<PersonUpdateViewModel>().Create();
        var request = new PersonUpdateRequest(personUpdateViewModel, _user);

        _unitOfWorkMock.SetupSequence(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, false, default, default))
            .ReturnsAsync(null as Person)
            .ReturnsAsync(_fixture.Build<Person>().With(x => x.Country, null as Country).Create());
        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, false, default, default))
            .ReturnsAsync(_fixture.Build<Country>().With(x => x.People, null as List<Person>).Create());
        _unitOfWorkMock.Setup(x => x.LastSaveChangeResult).Returns(_fixture.Build<SaveChangesResult>().With(x => x.Exception, new ContactManagerSaveDatabaseException()).Create());

        var handler = new PersonUpdateRequestHandler(_unitOfWorkMock.Object, _mapper, _dateCalculatorMock.Object, _cacheServiceMock.Object, _loggerMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeFalse();
        result.Exception.Should().BeOfType<ContactManagerSaveDatabaseException>();
    }

    [Fact]
    [Trait("UpdateHandleTests", nameof(Person))]
    public async Task PersonUpdateRequestHandler_Should_ReturnSuccessResult_WhenAllPropertiesAreCorrect()
    {
        var personUpdateViewModel = _fixture.Build<PersonUpdateViewModel>().With(x => x.Gender, GenderOptions.None).Create();

        var expected = new PersonViewModel()
        {
            Id = personUpdateViewModel.Id,
            FirstName = personUpdateViewModel.FirstName,
            LastName = personUpdateViewModel.LastName,
            Email = personUpdateViewModel.Email,
            Phone = personUpdateViewModel.Phone,
            DateOfBirth = personUpdateViewModel.DateOfBirth,
            Address = personUpdateViewModel.Address,
            Gender = "None",
            CountryName = "country"
        };

        var request = new PersonUpdateRequest(personUpdateViewModel, _user);

        _unitOfWorkMock.SetupSequence(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, false, default, default))
            .ReturnsAsync(null as Person)
            .ReturnsAsync(_fixture.Build<Person>().With(x => x.Id, personUpdateViewModel.Id).With(x => x.Country, null as Country).Create());
        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, false, default, default))
            .ReturnsAsync(_fixture.Build<Country>().With(x => x.Name, "country").With(x => x.People, null as List<Person>).Create());
        _unitOfWorkMock.Setup(x => x.LastSaveChangeResult).Returns(new SaveChangesResult());

        var handler = new PersonUpdateRequestHandler(_unitOfWorkMock.Object, _mapper, _dateCalculatorMock.Object, _cacheServiceMock.Object, _loggerMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeTrue();
        result.Result.Should().BeEquivalentTo(expected);
    }


}
