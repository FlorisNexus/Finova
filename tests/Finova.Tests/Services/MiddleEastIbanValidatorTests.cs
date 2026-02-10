using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class MiddleEastIbanValidatorTests
{
    private readonly MiddleEastIbanValidator _validator;

    public MiddleEastIbanValidatorTests()
    {
        _validator = new MiddleEastIbanValidator();
    }

    [Theory]
    [InlineData("AE070331234567890123456")] // UAE
    public void ValidateIban_WithValidMiddleEastIban_ReturnsTrue(string iban)
    {
        var result = MiddleEastIbanValidator.ValidateIban(iban);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("AE070331234567890123456")] // UAE
    public void Validate_Instance_WithValidMiddleEastIban_ReturnsTrue(string iban)
    {
        var result = _validator.Validate(iban);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateIban_WithNull_ReturnsFalse()
    {
        var result = MiddleEastIbanValidator.ValidateIban(null);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Fact]
    public void ValidateIban_WithUnsupportedCountry_ReturnsUnsupportedCountry()
    {
        var result = MiddleEastIbanValidator.ValidateIban("ZZ000000000000000000");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    [Theory]
    [InlineData("AE", true)]  // UAE
    [InlineData("SA", true)]  // Saudi Arabia
    [InlineData("QA", true)]  // Qatar
    [InlineData("JO", true)]  // Jordan
    [InlineData("BE", false)] // Belgium - not Middle Eastern
    [InlineData("US", false)] // US - not supported
    public void IsCountrySupported_ReturnsExpected(string countryCode, bool expected)
    {
        MiddleEastIbanValidator.IsCountrySupported(countryCode).Should().Be(expected);
    }
}
