using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class AsiaIbanValidatorTests
{
    private readonly AsiaIbanValidator _validator;

    public AsiaIbanValidatorTests()
    {
        _validator = new AsiaIbanValidator();
    }

    [Theory]
    [InlineData("KZ86125KZT5004100100")] // Kazakhstan
    [InlineData("PK36SCBL0000001123456702")] // Pakistan
    public void ValidateIban_WithValidAsiaIban_ReturnsTrue(string iban)
    {
        var result = AsiaIbanValidator.ValidateIban(iban);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("KZ86125KZT5004100100")] // Kazakhstan
    [InlineData("PK36SCBL0000001123456702")] // Pakistan
    public void Validate_Instance_WithValidAsiaIban_ReturnsTrue(string iban)
    {
        var result = _validator.Validate(iban);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateIban_WithNull_ReturnsFalse()
    {
        var result = AsiaIbanValidator.ValidateIban(null);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Fact]
    public void ValidateIban_WithUnsupportedCountry_ReturnsUnsupportedCountry()
    {
        var result = AsiaIbanValidator.ValidateIban("ZZ000000000000000000");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    [Theory]
    [InlineData("KZ", true)]  // Kazakhstan
    [InlineData("PK", true)]  // Pakistan
    [InlineData("MN", true)]  // Mongolia
    [InlineData("BE", false)] // Belgium - not Asian
    [InlineData("BR", false)] // Brazil - not Asian
    public void IsCountrySupported_ReturnsExpected(string countryCode, bool expected)
    {
        AsiaIbanValidator.IsCountrySupported(countryCode).Should().Be(expected);
    }
}
