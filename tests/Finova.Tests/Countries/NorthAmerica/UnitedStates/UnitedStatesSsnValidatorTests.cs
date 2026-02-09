using Finova.Core.Common;
using Finova.Countries.NorthAmerica.UnitedStates.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.UnitedStates;

public class UnitedStatesSsnValidatorTests
{
    private readonly UnitedStatesSsnValidator _validator = new();

    [Theory]
    [InlineData("000-00-0000", false)] // Invalid Area 000, Group 00, Serial 0000
    [InlineData("123-45-6789", true)]  // Valid format
    [InlineData("123456789", true)]    // Valid pure digits
    [InlineData("123 45 6789", true)]  // Valid with spaces
    [InlineData("666-12-3456", false)] // Invalid Area 666
    [InlineData("900-12-3456", false)] // Invalid Area 900+
    [InlineData("123-00-6789", false)] // Invalid Group 00
    [InlineData("123-45-0000", false)] // Invalid Serial 0000
    [InlineData("ABC-DE-FGHI", false)] // Invalid characters
    [InlineData("123-45-678", false)]  // Too short
    [InlineData("123-45-67890", false)]// Too long
    [InlineData(null, false)]          // Null
    [InlineData("", false)]            // Empty
    public void Validate_ShouldReturnExpectedResult(string? ssn, bool expectedIsValid)
    {
        // Act
        var result = _validator.Validate(ssn);

        // Assert
        result.IsValid.Should().Be(expectedIsValid);
    }

    [Fact]
    public void Validate_WithInvalidArea_ReturnsInvalidFormat()
    {
        // Act
        var result = _validator.Validate("000-12-3456");

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be(ValidationMessages.InvalidSsnArea);
    }

    [Fact]
    public void Validate_WithInvalidGroup_ReturnsInvalidFormat()
    {
        // Act
        var result = _validator.Validate("123-00-6789");

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be(ValidationMessages.InvalidSsnGroup);
    }

    [Fact]
    public void Validate_WithInvalidSerial_ReturnsInvalidFormat()
    {
        // Act
        var result = _validator.Validate("123-45-0000");

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be(ValidationMessages.InvalidSsnSerial);
    }
}
