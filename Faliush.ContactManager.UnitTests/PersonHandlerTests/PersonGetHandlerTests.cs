using Faliush.ContactManager.Core.Enums;
using Faliush.ContactManager.Infrastructure.UnitOfWork.Pagination;

namespace Faliush.ContactManager.UnitTests.PersonHandlerTests;

public class PersonGetHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDateCalcualtorService> _dateCalculatorMock;
    private readonly Mock<ILogger<PersonGetFilteredRequestHandler>> _loggerGetFilteredMock;
    private readonly Mock<ILogger<PersonGetFilteredPagedRequestHandler>> _loggerGetFilteredPagedMock;
    private readonly Mock<ILogger<PersonGetByIdRequestHandler>> _loggerGetByIdMock;
    private readonly Mock<ILogger<PersonGetForUpdateRequestHandler>> _loggerGetForUpdateMock;
    private readonly IFixture _fixture;
    private readonly IMapper _mapper;

    public PersonGetHandlerTests()
    {
        _fixture = new Fixture();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dateCalculatorMock = new Mock<IDateCalcualtorService>();
        _loggerGetForUpdateMock = new Mock<ILogger<PersonGetForUpdateRequestHandler>>();
        _loggerGetFilteredPagedMock = new Mock<ILogger<PersonGetFilteredPagedRequestHandler>>();
        _loggerGetFilteredMock = new Mock<ILogger<PersonGetFilteredRequestHandler>>();
        _loggerGetByIdMock = new Mock<ILogger<PersonGetByIdRequestHandler>>();

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
    [Trait("GetHandleTests", nameof(Person))]
    public async Task PersonGetFilteredRequestHandler_Should_ReturnSomePeople_WhenDatabaseIsNotEmpty()
    {
        var sortBy = "LastName";
        var sortOrder = SortOptions.Asc;
        var request = new PersonGetFilteredRequest(null, null,sortBy, sortOrder);

        var people = new List<Person>()
        {
            _fixture.Build<Person>().With(x => x.Country, null as Country).Create(),
            _fixture.Build<Person>().With(x => x.Country, null as Country).Create(),
            _fixture.Build<Person>().With(x => x.Country, null as Country).Create(),
        };

        var expected = people.Select(x => new PeopleViewModel() { Id = x.Id, Email = x.Email, FirstName = x.FirstName, LastName = x.LastName }).ToList();

        _unitOfWorkMock.Setup(x => x.GetRepository<Person>()
            .GetAllAsync(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>(), default, default, default, default))
            .ReturnsAsync(people);

        var handler = new PersonGetFilteredRequestHandler(_unitOfWorkMock.Object, _mapper, _loggerGetFilteredMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeTrue();
        result.Result.Should().NotBeNullOrEmpty();
        result.Result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    [Trait("GetHandleTests", nameof(Person))]
    public async Task PersonGetFilteredPagedRequestHandler_Should_ReturnSomePeople_WhenDatabaseIsNotEmpty()
    {
        var indexPage = 0;
        var pageSize = 10;
        var sortBy = "LastName";
        var sortOrder = SortOptions.Asc;
        var request = new PersonGetFilteredPagedRequest(indexPage, pageSize, null, null, sortBy, sortOrder);

        var people = new List<Person>()
        {
            _fixture.Build<Person>().With(x => x.Country, null as Country).Create(),
            _fixture.Build<Person>().With(x => x.Country, null as Country).Create(),
            _fixture.Build<Person>().With(x => x.Country, null as Country).Create(),
        };

        var expected = people.Select(x => new PeopleViewModel() { Id = x.Id, Email = x.Email, FirstName = x.FirstName, LastName = x.LastName }).ToPagedList(indexPage, pageSize);

        _unitOfWorkMock.Setup(x => x.GetRepository<Person>()
            .GetPagedListAsync(
                It.IsAny<Expression<Func<Person, bool>>>(), 
                It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>(), 
                default,
                indexPage, 
                pageSize, 
                It.IsAny<bool>(), 
                default, 
                default))
            .ReturnsAsync(people.ToPagedList(indexPage, pageSize));

        var handler = new PersonGetFilteredPagedRequestHandler(_unitOfWorkMock.Object, _mapper, _loggerGetFilteredPagedMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeTrue();
        result.Result.Should().NotBeNull();
        result.Result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    [Trait("GetHandleTests", nameof(Person))]
    public async Task PersonGetByIdRequestHandler_Should_ThrowContactManagerNotFoundException_WhenPersonIdIsNotExist()
    {
        var personId = Guid.Parse("30D66FCD-B6B7-4E6E-B267-A184E81659A4");
        var request = new PersonGetByIdRequest(personId);

        _unitOfWorkMock.Setup(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, It.IsAny<bool>(), default, default))
            .ReturnsAsync(null as Person);

        var handler = new PersonGetByIdRequestHandler(_unitOfWorkMock.Object, _mapper, _dateCalculatorMock.Object, _loggerGetByIdMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeFalse();
        result.Exception.Should().BeOfType<ContactManagerNotFoundException>();  
    }

    [Fact]
    [Trait("GetHandleTests", nameof(Person))]
    public async Task PersonGetByIdRequest_Should_ReturnSuccessResult_WhenPersonIdIsCorrect()
    {
        var personId = Guid.Parse("30D66FCD-B6B7-4E6E-B267-A184E81659A4");
        var request = new PersonGetByIdRequest(personId);

        var person = _fixture.Build<Person>().With(x => x.Id, personId).With(x => x.Country, null as Country).Create();
        var expected = new PersonViewModel()
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            Email = person.Email,
            Phone = person.Phone,
            Address = person.Address,
            Gender = person.Gender.ToString(),
            Age = 20
        };

        _dateCalculatorMock.Setup(x => x.GetTotalYears(It.IsAny<DateTime>())).Returns(20);
        _unitOfWorkMock.Setup(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, It.IsAny<bool>(), default, default))
            .ReturnsAsync(person);

        var handler = new PersonGetByIdRequestHandler(_unitOfWorkMock.Object, _mapper, _dateCalculatorMock.Object, _loggerGetByIdMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeTrue();
        result.Result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    [Trait("GetHandleTests", nameof(Person))]
    public async Task PersonGetForUpdateHandler_Should_ThrowContactManagerNotFoundException_WhenPersonIdDoesNotExist()
    {
        var personId = Guid.Parse("30D66FCD-B6B7-4E6E-B267-A184E81659A4");
        var request = new PersonGetForUpdateRequest(personId);

        _unitOfWorkMock.Setup(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, It.IsAny<bool>(), default, default))
            .ReturnsAsync(null as Person);

        var handler = new PersonGetForUpdateRequestHandler(_unitOfWorkMock.Object, _mapper, _loggerGetForUpdateMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeFalse();
        result.Exception.Should().BeOfType<ContactManagerNotFoundException>();
    }

    [Fact]
    [Trait("GetHandleTests", nameof(Person))]
    public async Task PersonGetForUpdateRequest_Should_ReturnSuccessResult_WhenPersonIdIsCorrect()
    {
        var personId = Guid.Parse("30D66FCD-B6B7-4E6E-B267-A184E81659A4");
        var request = new PersonGetForUpdateRequest(personId);

        var person = _fixture.Build<Person>().With(x => x.Id, personId).With(x => x.Country, null as Country).Create();
        var expected = new PersonUpdateViewModel()
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            Email = person.Email,
            Phone = person.Phone,
            Address = person.Address,
            Gender = person.Gender,
            CountryId = person.CountryId
        };

        _dateCalculatorMock.Setup(x => x.GetTotalYears(It.IsAny<DateTime>())).Returns(20);
        _unitOfWorkMock.Setup(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, It.IsAny<bool>(), default, default))
            .ReturnsAsync(person);

        var handler = new PersonGetForUpdateRequestHandler(_unitOfWorkMock.Object, _mapper, _loggerGetForUpdateMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeTrue();
        result.Result.Should().BeEquivalentTo(expected);
    }

}
