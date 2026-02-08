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
    public void ValidateIban_WithNullIban_ReturnsFalse()
    {
        GlobalIbanValidator.ValidateIban(null).IsValid.Should().BeFalse();
    }

    [Fact]
    public void ValidateIban_WithInvalidIban_ReturnsFalse()
    {
        GlobalIbanValidator.ValidateIban("invalid").IsValid.Should().BeFalse();
    }

    [Fact]
    public void ValidateIban_WithUnsupportedCountry_ReturnsFalse()
    {
        // Use a country code that is definitely not supported and a string that is too short to be a valid IBAN
        GlobalIbanValidator.ValidateIban("ZZ123").IsValid.Should().BeFalse();
    }
}