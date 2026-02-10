using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class AmericasVatValidatorTests
{
    [Theory]
    [InlineData("AR", "AR20123456789")] // Argentina
    [InlineData("BR", "BR12345678000190")] // Brazil
    [InlineData("CL", "CL123456785")] // Chile
    [InlineData("CO", "CO1234567891")] // Colombia
    [InlineData("MX", "MXABC123456789")] // Mexico
    public void ValidateVat_ShouldDelegateToCorrectValidator(string countryCode, string vatNumber)
    {
        var result = AmericasVatValidator.ValidateVat(vatNumber);
        result.Errors.Should().NotContain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
        vatNumber.Should().StartWith(countryCode);
    }

    [Fact]
    public void ValidateVat_ShouldReturnUnsupportedCountry_ForUnknownCountry()
    {
        var result = AmericasVatValidator.ValidateVat("ZZ123456789");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }
}
