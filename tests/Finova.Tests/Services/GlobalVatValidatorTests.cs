using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class GlobalVatValidatorTests
{
    [Theory]
    [InlineData("BE0123456789")] // Europe (Belgium)
    [InlineData("ZA1234567890")] // Africa (South Africa)
    [InlineData("CN123456789012345678")] // Asia (China)
    [InlineData("BR12345678000190")] // Americas (Brazil)
    [InlineData("AE123456789012345")] // Middle East (UAE)
    [InlineData("AU12345678901")] // Oceania (Australia)
    public void ValidateVat_ShouldRouteToCorrectContinent(string vatNumber)
    {
        var result = GlobalVatValidator.ValidateVat(vatNumber);
        result.Errors.Should().NotContain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    [Fact]
    public void ValidateVat_ShouldReturnUnsupportedCountry_ForUnknownCountry()
    {
        var result = GlobalVatValidator.ValidateVat("ZZ123456789");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }
}
