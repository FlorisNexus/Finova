using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class AsiaVatValidatorTests
{
    [Theory]
    [InlineData("CN", "CN123456789012345678")] // China
    [InlineData("IN", "IN27AAAAA0000A1Z5")] // India
    [InlineData("JP", "JP1234567890123")] // Japan
    [InlineData("KR", "KR1234567890")] // South Korea
    [InlineData("SG", "SG123456789G")] // Singapore
    public void ValidateVat_ShouldDelegateToCorrectValidator(string countryCode, string vatNumber)
    {
        var result = AsiaVatValidator.ValidateVat(vatNumber);
        result.Errors.Should().NotContain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
        vatNumber.Should().StartWith(countryCode);
    }

    [Fact]
    public void ValidateVat_ShouldReturnUnsupportedCountry_ForUnknownCountry()
    {
        var result = AsiaVatValidator.ValidateVat("ZZ123456789");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }
}
