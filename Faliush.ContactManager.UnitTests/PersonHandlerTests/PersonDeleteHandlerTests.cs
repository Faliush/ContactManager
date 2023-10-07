namespace Faliush.ContactManager.UnitTests.PersonHandlerTests;

public class PersonDeleteHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IFixture _fixture;

    public PersonDeleteHandlerTests()
    {
        _fixture = new Fixture();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    [Trait("DeleteHandleTests", nameof(Person))]
    public async Task PersonDeleteRequestHandler_Should_ThrowContactManagerNotFoundException_WhenPersonIdIsNotExist()
    {
        var personId = Guid.Parse("E72CBF25-519D-43B8-8CB1-F9C36B83A7CB");
        var request = new PersonDeleteRequest(personId);

        _unitOfWorkMock.Setup(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, default, default, default))
            .ReturnsAsync(null as Person);

        var handler = new PersonDeleteRequestHandler(_unitOfWorkMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeFalse();
        result.Exception.Should().BeOfType<ContactManagerNotFoundException>();
    }

    [Fact]
    [Trait("DeleteHandleTests", nameof(Person))]
    public async Task PersonDeleteRequestHandler_Should_ThrowContactManagerDatabaseSaveException_WhenSaveChangeResultIsNotOk()
    {
        var personId = Guid.Parse("E72CBF25-519D-43B8-8CB1-F9C36B83A7CB");
        var request = new PersonDeleteRequest(personId);

        _unitOfWorkMock.Setup(x => x.LastSaveChangeResult).Returns(_fixture.Build<SaveChangesResult>().With(x => x.Exception, new ContactManagerSaveDatabaseException()).Create());
        _unitOfWorkMock.Setup(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, default, default, default))
            .ReturnsAsync(_fixture.Build<Person>().With(x => x.Id, personId).With(x => x.Country, null as Country).Create());

        var handler = new PersonDeleteRequestHandler(_unitOfWorkMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeFalse();
        result.Exception.Should().BeOfType<ContactManagerSaveDatabaseException>();
    }

    [Fact]
    [Trait("DeleteHandleTests", nameof(Person))]
    public async Task PersonDeleteRequestHandler_Should_ReturnSuccessResult_WhenPersonIdIsCorrectAndExist()
    {
        var personId = Guid.Parse("E72CBF25-519D-43B8-8CB1-F9C36B83A7CB");
        var request = new PersonDeleteRequest(personId);

        _unitOfWorkMock.Setup(x => x.LastSaveChangeResult).Returns(new SaveChangesResult());
        _unitOfWorkMock.Setup(x => x.GetRepository<Person>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Person, bool>>>(), default, default, default, default, default))
            .ReturnsAsync(_fixture.Build<Person>().With(x => x.Id, personId).With(x =>  x.Country, null as Country).Create());

        var handler = new PersonDeleteRequestHandler(_unitOfWorkMock.Object);

        var result = await handler.Handle(request, default);

        result.Ok.Should().BeTrue();
        result.Result.Should().Be(personId);
    }

}
