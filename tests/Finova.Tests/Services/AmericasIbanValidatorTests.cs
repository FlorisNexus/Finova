using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class AmericasIbanValidatorTests
{
    private readonly AmericasIbanValidator _validator;

    public AmericasIbanValidatorTests()
    {
        _validator = new AmericasIbanValidator();
    }

    [Theory]
    [InlineData("BR1800360305000010009795493C1")] // Brazil
    [InlineData("CR05015202001026284066")] // Costa Rica
    public void ValidateIban_WithValidAmericasIban_ReturnsTrue(string iban)
    {
        var result = AmericasIbanValidator.ValidateIban(iban);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("BR1800360305000010009795493C1")] // Brazil
    [InlineData("CR05015202001026284066")] // Costa Rica
    public void Validate_Instance_WithValidAmericasIban_ReturnsTrue(string iban)
    {
        var result = _validator.Validate(iban);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateIban_WithNull_ReturnsFalse()
    {
        var result = AmericasIbanValidator.ValidateIban(null);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Fact]
    public void ValidateIban_WithUnsupportedCountry_ReturnsUnsupportedCountry()
    {
        var result = AmericasIbanValidator.ValidateIban("ZZ000000000000000000");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    [Theory]
    [InlineData("BR", true)]  // Brazil
    [InlineData("CR", true)]  // Costa Rica
    [InlineData("GT", true)]  // Guatemala
    [InlineData("BE", false)] // Belgium - not American
    [InlineData("JP", false)] // Japan - not American
    public void IsCountrySupported_ReturnsExpected(string countryCode, bool expected)
    {
        AmericasIbanValidator.IsCountrySupported(countryCode).Should().Be(expected);
    }
}
