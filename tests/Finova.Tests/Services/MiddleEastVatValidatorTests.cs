using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class MiddleEastVatValidatorTests
{
    [Theory]
    [InlineData("AE", "AE123456789012345")] // UAE
    [InlineData("SA", "SA123456789012345")] // Saudi Arabia
    [InlineData("IL", "IL123456789")] // Israel
    public void ValidateVat_ShouldDelegateToCorrectValidator(string countryCode, string vatNumber)
    {
        var result = MiddleEastVatValidator.ValidateVat(vatNumber);
        result.Errors.Should().NotContain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
        vatNumber.Should().StartWith(countryCode);
    }

    [Fact]
    public void ValidateVat_ShouldReturnUnsupportedCountry_ForUnknownCountry()
    {
        var result = MiddleEastVatValidator.ValidateVat("ZZ123456789");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }
}
