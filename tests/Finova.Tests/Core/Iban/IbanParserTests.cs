using Finova.Core.Iban;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Core.Iban;

public class IbanParserTests
{
    private readonly IbanParser _parser;

    public IbanParserTests()
    {
        _parser = new IbanParser();
    }

    [Fact]
    public void CountryCode_ShouldBeNull()
    {
        // Generic parser should not have a country code
        _parser.CountryCode.Should().BeNull();
    }

    [Theory]
    [InlineData("BE68539007547034", "BE", "68")]
    [InlineData("NL91ABNA0417164300", "NL", "91")]
    [InlineData("LU280019400644750000", "LU", "28")]
    [InlineData("GB29NWBK60161331926819", "GB", "29")]
    [InlineData("FR1420041010050500013M02606", "FR", "14")]
    [InlineData("DE89370400440532013000", "DE", "89")]
    public void Parse_WithValidIban_ReturnsDetails(string iban, string expectedCountryCode, string expectedCheckDigits)
    {
        // Act
        var result = IbanParser.Parse(iban);

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be(iban.ToUpperInvariant().Replace(" ", ""));
        result.CountryCode.Should().Be(expectedCountryCode);
        result.CheckDigits.Should().Be(expectedCheckDigits);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("BE 68 5390 0754 7034", "BE", "68")] // With spaces
    [InlineData("be68539007547034", "BE", "68")] // Lowercase
    [InlineData("BE68 5390 0754 7034", "BE", "68")] // Mixed formatting
    public void Parse_WithFormattedIban_ReturnsNormalizedDetails(string iban, string expectedCountryCode, string expectedCheckDigits)
    {
        // Act
        var result = IbanParser.Parse(iban);

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be(iban.ToUpperInvariant().Replace(" ", ""));
        result.CountryCode.Should().Be(expectedCountryCode);
        result.CheckDigits.Should().Be(expectedCheckDigits);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("XX68539007547034")] // Invalid country code
    [InlineData("BE00539007547034")] // Invalid check digits
    [InlineData("INVALID")]
    [InlineData("1234567890")]
    public void Parse_WithInvalidIban_ReturnsNull(string? iban)
    {
        // Act
        var result = IbanParser.Parse(iban);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("BE68539007547034", "BE", "68")]
    [InlineData("NL91ABNA0417164300", "NL", "91")]
    public void ParseIban_WithValidIban_ReturnsDetails(string iban, string expectedCountryCode, string expectedCheckDigits)
    {
        // Act
        var result = _parser.ParseIban(iban);

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be(iban.ToUpperInvariant().Replace(" ", ""));
        result.CountryCode.Should().Be(expectedCountryCode);
        result.CheckDigits.Should().Be(expectedCheckDigits);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("INVALID")]
    public void ParseIban_WithInvalidIban_ReturnsNull(string? iban)
    {
        // Act
        var result = _parser.ParseIban(iban);

        // Assert
        result.Should().BeNull();
    }
}
