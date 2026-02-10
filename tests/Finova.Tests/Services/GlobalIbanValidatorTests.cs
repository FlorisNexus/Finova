using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Services.Global;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class GlobalIbanValidatorTests
{
    [Theory]
    [InlineData("EG380019000500000000263180002")] // Egypt (Valid from EG tests)
    [InlineData("MR1300020001010000123456753")] // Mauritania (Valid from MR tests)
    [InlineData("KZ86125KZT5004100100")] // Kazakhstan (Valid from KZ tests)
    [InlineData("BR1800360305000010009795493C1")] // Brazil (Valid from BR tests)
    [InlineData("AE070331234567890123456")] // UAE (Valid from AE tests)
    [InlineData("SA0380000000608010167519")] // Saudi Arabia (Valid from SA tests)
    [InlineData("CR05015202001026284066")] // Costa Rica (North America)
    [InlineData("DO22ACAU00000000000123456789")] // Dominican Republic (North America)
    [InlineData("BE68539007547034")] // Belgium (Europe)
    public void ValidateIban_WithValidGlobalIban_ReturnsTrue(string iban)
    {
        // Act
        var result = GlobalIbanValidator.ValidateIban(iban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateIban_WithNullIban_ReturnsInvalidInput()
    {
        var result = GlobalIbanValidator.ValidateIban(null);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Fact]
    public void ValidateIban_WithTooShortIban_ReturnsInvalidLength()
    {
        // "XX00123" is 7 chars, min is 15. Structure is valid (XX letters, 00 digits).
        var result = GlobalIbanValidator.ValidateIban("XX00123");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidLength);
    }

    [Fact]
    public void ValidateIban_WithTooLongIban_ReturnsInvalidLength()
    {
        // 35 chars
        var result = GlobalIbanValidator.ValidateIban("BE68539007547034BE68539007547034123");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidLength);
    }

    [Fact]
    public void ValidateIban_WithInvalidCountryCode_ReturnsInvalidCountryCode()
    {
        // Starts with digits: 1268539007547034 (16 chars, valid length)
        var result = GlobalIbanValidator.ValidateIban("1268539007547034");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidCountryCode);
    }

    [Fact]
    public void ValidateIban_WithInvalidCheckDigits_ReturnsInvalidCheckDigit()
    {
        // Check digits are letters: BEXX539007547034
        var result = GlobalIbanValidator.ValidateIban("BEXX539007547034");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidCheckDigit);
    }

    [Fact]
    public void ValidateIban_WithInvalidFormat_ReturnsInvalidFormat()
    {
        // Contains special char: BE6853900754703-
        var result = GlobalIbanValidator.ValidateIban("BE6853900754703-");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Fact]
    public void ValidateIban_WithInvalidChecksum_ReturnsInvalidChecksum()
    {
        // BE68 5390 0754 7034 is valid. Change last digit to 5.
        // BE68 5390 0754 7035 -> Checksum should fail
        var result = GlobalIbanValidator.ValidateIban("BE68539007547035");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidChecksum);
    }

    [Fact]
    public void ValidateIban_WithBelgiumIbanInvalidLength_ReturnsInvalidLengthError()
    {
        // BE68 5390 0754 70345 (17 chars, expected 16)
        // This should fail on length, NOT checksum.
        var result = GlobalIbanValidator.ValidateIban("BE685390075470345");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidLength);
        result.Errors.Should().NotContain(e => e.Code == ValidationErrorCode.InvalidChecksum);
    }

    [Fact]
    public void ValidateIban_WithShortGermanIban_ReturnsFormattedErrorMessage()
    {
        // DE89 37546456 (12 chars).
        // Previously this hit global check (min 15).
        // NOW it should hit Germany validator (Expected 22).
        var result = GlobalIbanValidator.ValidateIban("DE8937546456");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidLength);

        var error = result.Errors.First(e => e.Code == ValidationErrorCode.InvalidLength);
        // "Invalid length. Expected 22, got 12."
        error.Message.Should().Contain("22");
        error.Message.Should().NotContain("15-34");
    }

    [Fact]
    public void Validate_Instance_WithRegisteredValidator_RoutesCorrectly()
    {
        // Arrange
        var mockValidator = new MockIbanValidator("ZZ");
        var globalValidator = new GlobalIbanValidator(new[] { mockValidator });

        // Act
        var result = globalValidator.Validate("ZZ123456789012345");

        // Assert
        result.IsValid.Should().BeTrue();
        mockValidator.WasCalled.Should().BeTrue();
    }

    [Fact]
    public void Validate_Instance_WithUnregisteredCountry_ReturnsUnsupported()
    {
        // Arrange
        var globalValidator = new GlobalIbanValidator(Enumerable.Empty<IIbanValidator>());

        // Act
        var result = globalValidator.Validate("BE68539007547034");

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    private class MockIbanValidator : Finova.Core.Iban.IIbanValidator
    {
        public MockIbanValidator(string countryCode) => CountryCode = countryCode;
        public string CountryCode { get; }
        public bool WasCalled { get; private set; }
        public ValidationResult Validate(string? iban)
        {
            WasCalled = true;
            return ValidationResult.Success();
        }
        public string FormatIban(string? iban) => iban ?? "";
        public string NormalizeIban(string? iban) => iban ?? "";
        public string GetCountryCode(string? iban) => CountryCode;
        public int GetCheckDigits(string? iban) => 0;
        public bool ValidateChecksum(string? iban) => true;
    }
}
