using Faliush.ContactManager.Core.Enums;

namespace Faliush.ContactManager.UnitTests.ServiceTests;

public class StringConvertServiceTests
{
    private readonly IStringConvertService _stringConvertService;

    public StringConvertServiceTests()
    {
        _stringConvertService = new StringConvertService(); 
    }

    [Fact]
    [Trait("ServiceTests", nameof(StringConvertService))]
    public void StringCovertService_Should_ThrowContactManagerInvalidOperationException_WhenGivenStringDoesNotExistInEnum()
    {
        var value = "something";

        var result = () => _stringConvertService.ConvertToEnum<SortOptions>(value);

        result.Should().Throw<ContactManagerInvalidOperationException>();
    }

    [Fact]
    [Trait("ServiceTests", nameof(StringConvertService))]
    public void StringCovertService_Should_ReturnEnum_WhenGivenStringExistsInEnum()
    {
        var value = "Asc";

        var result = _stringConvertService.ConvertToEnum<SortOptions>(value);

        result.Should().Be(SortOptions.Asc);
    }
}
