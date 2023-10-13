using System.Security.Claims;

namespace Faliush.ContactManager.UnitTests.PersonHandlerTests;

public class PersonCreateHandlerTests 
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDateCalcualtorService> _dateCalculatorMock;
    private readonly Mock<ILogger<PersonCreateRequestHandler>> _loggerMock;
    private readonly ClaimsPrincipal _user; 
    private readonly IFixture _fixture;
    private readonly IMapper _mapper;

    public PersonCreateHandlerTests()
    {
        _fixture = new Fixture();   
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dateCalculatorMock = new Mock<IDateCalcualtorService>();
        _loggerMock = new Mock<ILogger<PersonCreateRequestHandler>>();
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
    [Trait("CreateHandleTests", nameof(Person))]
    public async Task PersonCreateRequestHandler_Should_ThrowContactManagerArgumentException_WhenEmailIsNotUnique()
    {
        var personCreateViewModel = _fixture.Build<PersonCreateViewModel>()
            .With(x => x.Email, "test@gmail.com").Create();

        var request = new PersonCreateRequest(personCreateViewModel, _user);

        _unitOfWorkMock.Setup(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, default, default, default))
            .ReturnsAsync(_fixture.Build<Person>().With(x => x.Email, "test@gmail.com").With(x => x.Country, null as Country).Create());

        var handler = new PersonCreateRequestHandler(_unitOfWorkMock.Object, _mapper, _dateCalculatorMock.Object, _loggerMock.Object);

        var result = await handler.Handle(request, default);

        result.Exception.Should().BeOfType<ContactManagerArgumentException>();
    }

    [Fact]
    [Trait("CreateHandleTests", nameof(Person))]
    public async Task PersonCreateRequestHandler_Should_ThrowContactManagerArgumentException_WhenCountryIsNotFound()
    {
        var personCreateViewModel = _fixture.Build<PersonCreateViewModel>().Create();

        var request = new PersonCreateRequest(personCreateViewModel, _user);


        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, default, default, default))
             .ReturnsAsync(null as Country);
        _unitOfWorkMock.Setup(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, default, default, default))
            .ReturnsAsync(null as Person);

        var handler = new PersonCreateRequestHandler(_unitOfWorkMock.Object, _mapper, _dateCalculatorMock.Object, _loggerMock.Object);

        var result = await handler.Handle(request, default);

        result.Exception.Should().BeOfType<ContactManagerNotFoundException>();
    }

    [Fact]
    [Trait("CreateHandleTests", nameof(Person))]
    public async Task PersonCreateRequestHandler_Should_ThrowContactManagerDatabaseSaveException_WhenSaveChangeResultIsNotOk()
    {
        var personCreateViewModel = _fixture.Build<PersonCreateViewModel>().Create();

        var request = new PersonCreateRequest(personCreateViewModel, _user);

        _unitOfWorkMock.Setup(x => x.LastSaveChangeResult).Returns(_fixture.Build<SaveChangesResult>().With(x => x.Exception, new ContactManagerSaveDatabaseException()).Create());
        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, default, default, default))
             .ReturnsAsync(_fixture.Build<Country>().With(x => x.People, null as List<Person>).Create());
        _unitOfWorkMock.Setup(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, default, default, default))
            .ReturnsAsync(null as Person);

        var handler = new PersonCreateRequestHandler(_unitOfWorkMock.Object, _mapper, _dateCalculatorMock.Object, _loggerMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeFalse();
        result.Exception.Should().BeOfType<ContactManagerSaveDatabaseException>();
    }

    [Fact]
    [Trait("CreateHandleTests", nameof(Person))]
    public async Task PersonCreateRequestHandler_Should_ReturnSuccessResult_WhenAllPropertiesAreCorrect()
    {
        var personCreateViewModel = _fixture.Build<PersonCreateViewModel>().With(x => x.Gender, GenderOptions.None).Create();

        var expected = new PersonViewModel() 
        {
            FirstName = personCreateViewModel.FirstName,
            LastName = personCreateViewModel.LastName,
            Email = personCreateViewModel.Email,    
            Phone = personCreateViewModel.Phone,
            DateOfBirth = personCreateViewModel.DateOfBirth,
            Address = personCreateViewModel.Address,
            Gender = "None",
            CountryName = "country"
        };

        var request = new PersonCreateRequest(personCreateViewModel, _user);

        _unitOfWorkMock.Setup(x => x.LastSaveChangeResult).Returns(new SaveChangesResult());
        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, default, default, default))
             .ReturnsAsync(_fixture.Build<Country>().With(x => x.Name, "country").With(x => x.People, null as List<Person>).Create());
        _unitOfWorkMock.Setup(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, default, default, default))
            .ReturnsAsync(null as Person);

        var handler = new PersonCreateRequestHandler(_unitOfWorkMock.Object, _mapper, _dateCalculatorMock.Object, _loggerMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeTrue();
        result.Result.Should().BeEquivalentTo(expected);
    }

}
