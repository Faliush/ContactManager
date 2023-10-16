using Faliush.ContactManager.Core.Services.Implementations;
using Faliush.ContactManager.Core.Services.Interfaces;

namespace Faliush.ContactManager.UnitTests.ServiceTests;

public class DateCalculatorServiceTests
{
    private readonly IDateCalcualtorService _dateTimeCalculatorService;

    public DateCalculatorServiceTests()
    {
        _dateTimeCalculatorService = new DateCalculatorService();
    }

    [Fact]
    [Trait("ServiceTests", nameof(DateCalculatorService))]
    public void DateCalculatorService_ShouldReturnProperAge_WhenDateTimeIsvalid()
    {
        var date = DateTime.Parse("08.12.2004");
        var expected = 18;

        var result =  _dateTimeCalculatorService.GetTotalYears(date);

        result.Should().Be(expected);
    }
}
