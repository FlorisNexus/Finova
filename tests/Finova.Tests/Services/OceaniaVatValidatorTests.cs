using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class OceaniaVatValidatorTests
{
    [Theory]
    [InlineData("AU", "AU12345678901")] // Australia
    [InlineData("NZ", "NZ123456789")] // New Zealand
    public void ValidateVat_ShouldDelegateToCorrectValidator(string countryCode, string vatNumber)
    {
        var result = OceaniaVatValidator.ValidateVat(vatNumber);
        result.Errors.Should().NotContain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
        vatNumber.Should().StartWith(countryCode);
    }

    [Fact]
    public void ValidateVat_ShouldReturnUnsupportedCountry_ForUnknownCountry()
    {
        var result = OceaniaVatValidator.ValidateVat("ZZ123456789");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }
}
