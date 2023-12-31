﻿namespace Faliush.ContactManager.UnitTests.CountryHandlerTests;

public class CountryCreateHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ILogger<CountryCreateRequestHandler>> _loggerMock;
    private readonly IFixture _fixture;

    public CountryCreateHandlerTest()
    {
        _fixture = new Fixture();   
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _cacheServiceMock = new Mock<ICacheService>();
        _loggerMock = new Mock<ILogger<CountryCreateRequestHandler>>();
    }

    [Fact]
    [Trait("CreateHandleTests", nameof(Country))]
    public async Task CountryCreateRequestHandler_Should_ThrowContactManagerArgumentException_WhenCountryNameAlreadyExists()
    {
        // arrrange 
        var countryCreateViewModel = _fixture.Build<CountryCreateViewModel>().With(x => x.Name, "country").Create();
        var request = new CountryCreateRequest(countryCreateViewModel);

        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, default, default, default))
             .ReturnsAsync(new Country() { Name = "country" });

        var handler = new CountryCreateRequestHandler(_unitOfWorkMock.Object, _cacheServiceMock.Object, _loggerMock.Object);

        // act
        var result = await handler.Handle(request, default);

        // assert
        result.Should().NotBeNull();
        result.Exception.Should().BeOfType<ContactManagerArgumentException>();
    }

    [Fact]
    [Trait("CreateHandleTests", nameof(Country))]
    public async Task CountryCreateRequestHandler_Should_ReturnSuccessResult_WhenCountryNameIsUnique()
    {
        // arrrange 
        var countryCreateViewModel = _fixture.Build<CountryCreateViewModel>().Create();
        var request = new CountryCreateRequest(countryCreateViewModel);

        _unitOfWorkMock.Setup(x => x.LastSaveChangeResult).Returns(new SaveChangesResult());
        _unitOfWorkMock.Setup(x => x.GetRepository<Country>().GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>(), default, default, default, default, default))
            .ReturnsAsync(null as Country);

        var handler = new CountryCreateRequestHandler(_unitOfWorkMock.Object, _cacheServiceMock.Object, _loggerMock.Object);

        // act
        var result = await handler.Handle(request, default);


        // assert
        result.Ok.Should().BeTrue();
        result.Result.Should().NotBeNull();
        result.Result!.Name.Should().Be(countryCreateViewModel.Name);
    }

}
