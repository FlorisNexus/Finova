using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class AfricaIbanValidatorTests
{
    private readonly AfricaIbanValidator _validator;

    public AfricaIbanValidatorTests()
    {
        _validator = new AfricaIbanValidator();
    }

    // --- Happy flow: valid IBANs for each supported African country ---

    [Theory]
    [InlineData("EG380019000500000000263180002")] // Egypt
    [InlineData("MR1300020001010000123456753")] // Mauritania
    public void ValidateIban_WithValidAfricanIban_ReturnsTrue(string iban)
    {
        var result = AfricaIbanValidator.ValidateIban(iban);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("EG380019000500000000263180002")] // Egypt
    [InlineData("MR1300020001010000123456753")] // Mauritania
    [InlineData("TN5910006035183598478831")] // Tunisia
    public void Validate_Instance_WithValidAfricanIban_ReturnsTrue(string iban)
    {
        var result = _validator.Validate(iban);
        result.IsValid.Should().BeTrue();
    }

    // --- Unhappy flow ---

    [Fact]
    public void ValidateIban_WithNull_ReturnsFalse()
    {
        var result = AfricaIbanValidator.ValidateIban(null);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Fact]
    public void ValidateIban_WithEmptyString_ReturnsFalse()
    {
        var result = AfricaIbanValidator.ValidateIban("");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Fact]
    public void ValidateIban_WithSingleChar_ReturnsFalse()
    {
        var result = AfricaIbanValidator.ValidateIban("A");
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ValidateIban_WithUnsupportedCountry_ReturnsUnsupportedCountry()
    {
        var result = AfricaIbanValidator.ValidateIban("ZZ000000000000000000");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    [Fact]
    public void ValidateIban_WithEuropeanIban_ReturnsUnsupportedCountry()
    {
        var result = AfricaIbanValidator.ValidateIban("BE68539007547034");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    [Fact]
    public void Validate_Instance_WithNull_ReturnsFalse()
    {
        var result = _validator.Validate(null);
        result.IsValid.Should().BeFalse();
    }

    // --- IsCountrySupported ---

    [Theory]
    [InlineData("DZ", true)]  // Algeria
    [InlineData("EG", true)]  // Egypt
    [InlineData("TN", true)]  // Tunisia
    [InlineData("MA", true)]  // Morocco
    [InlineData("SD", true)]  // Sudan
    [InlineData("SC", true)]  // Seychelles
    [InlineData("DE", false)] // Germany - not African
    [InlineData("US", false)] // US - not supported
    [InlineData("XX", false)] // Non-existent
    public void IsCountrySupported_ReturnsExpected(string countryCode, bool expected)
    {
        AfricaIbanValidator.IsCountrySupported(countryCode).Should().Be(expected);
    }
}
