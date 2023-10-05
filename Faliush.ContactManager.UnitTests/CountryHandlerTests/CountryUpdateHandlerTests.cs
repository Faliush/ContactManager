﻿namespace Faliush.ContactManager.UnitTests.CountryHandlerTests;

public class CountryUpdateHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IFixture _fixture;

    public CountryUpdateHandlerTests()
    {
        _fixture = new Fixture();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    [Trait("UpdateHandleTests", nameof(Country))]
    public async Task Handler_Should_ThrowContactManagerNotFoundException_WhenCountryIdForUpdateIsNotCorrect()
    {
        var country = _fixture.Build<CountryUpdateViewModel>().Create();
        var request = new CountryUpdateRequest(country);

        _unitOfWorkMock.Setup(x => x.LastSaveChangeResult).Returns(new SaveChangesResult());
        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, default, default, default))
             .ReturnsAsync(null as Country);

        var handler = new CountryUpdateRequestHandler(_unitOfWorkMock.Object);

        var result = await handler.Handle(request, default);

        result.Exception.Should().BeOfType<ContactManagerNotFoundException>();
    }

    [Fact]
    [Trait("UpdateHandleTests", nameof(Country))]
    public async Task Handler_Should_ReturnSuccessResult_WhenIdAndNameCorrectForUpdating()
    {
        var country = _fixture.Build<CountryUpdateViewModel>().Create();
        var expected = new CountryViewModel() { Id = country.Id, Name = country.Name };
        var request = new CountryUpdateRequest(country);

        _unitOfWorkMock.Setup(x => x.LastSaveChangeResult).Returns(new SaveChangesResult());
        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, default, default, default))
             .ReturnsAsync(_fixture.Build<Country>().With(x => x.People, null as List<Person>).With(x => x.Id, country.Id).Create());

        var handler = new CountryUpdateRequestHandler(_unitOfWorkMock.Object);

        var result = await handler.Handle(request, default);

        result.Result.Should().BeEquivalentTo(expected);
    }
}
