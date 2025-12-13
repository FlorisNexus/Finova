using Finova.Core.Vat;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Core.Vat;

public class VatSanitizerTests
{
    [Theory]
    [InlineData("BE0123.456.789", "BE0123456789")]
    [InlineData("FR 12 345 678 901", "FR12345678901")]
    [InlineData("  IT  12345678901  ", "IT12345678901")]
    [InlineData("NL-1234.56.789.B01", "NL123456789B01")]
    [InlineData("abc-123", "ABC123")]
    public void Sanitize_WithSpecialCharacters_RemovesThemAndUppercases(string input, string expected)
    {
        // Act
        var result = VatSanitizer.Sanitize(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Sanitize_WithNullOrEmpty_ReturnsInput(string? input)
    {
        // Act
        var result = VatSanitizer.Sanitize(input);

        // Assert
        result.Should().Be(input);
    }
}
