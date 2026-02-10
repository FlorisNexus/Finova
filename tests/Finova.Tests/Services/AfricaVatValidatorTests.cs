using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class AfricaVatValidatorTests
{
    [Theory]
    [InlineData("ZA", "ZA1234567890")] // South Africa
    [InlineData("EG", "EG123456789")] // Egypt
    [InlineData("KE", "KEP051234567T")] // Kenya
    public void ValidateVat_ShouldDelegateToCorrectValidator(string countryCode, string vatNumber)
    {
        var result = AfricaVatValidator.ValidateVat(vatNumber);
        result.Errors.Should().NotContain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
        vatNumber.Should().StartWith(countryCode);
    }

    [Fact]
    public void ValidateVat_ShouldReturnUnsupportedCountry_ForUnknownCountry()
    {
        var result = AfricaVatValidator.ValidateVat("ZZ123456789");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    [Theory]
    [InlineData("ZA", "1234567890")]
    public void ValidateVat_WithExplicitCountryCode_ShouldValidate(string countryCode, string vatNumber)
    {
        var result = AfricaVatValidator.ValidateVat(vatNumber, countryCode);
        result.Errors.Should().NotContain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }
}
