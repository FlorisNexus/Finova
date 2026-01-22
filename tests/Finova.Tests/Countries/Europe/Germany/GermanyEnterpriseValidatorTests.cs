using Finova.Countries.Europe.Germany.Validators;
using Finova.Services;
using Xunit;

namespace Finova.Tests.Countries.Europe.Germany;

public class GermanyEnterpriseValidatorTests
{
    [Theory]
    [InlineData("1234567890123", true)] // 13 digits
    [InlineData("123456789012", false)] // 12 digits
    [InlineData("12345678901234", false)] // 14 digits
    [InlineData("123456789012A", false)] // Non-numeric
    [InlineData("", false)] // Empty
    [InlineData(null, false)] // Null
    [InlineData("12345", false)] // Too short - 5 digits
    [InlineData("123456", false)] // Too short - 6 digits
    public void Steuernummer_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = GermanySteuernummerValidator.ValidateSteuernummer(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("1234567890123", true)] // 13 digits - valid Steuernummer
    [InlineData("123456789012", false)] // 12 digits - invalid
    [InlineData("12345678901234", false)] // 14 digits - invalid
    [InlineData("12345", false)] // 5 digits - invalid (not HRB and not 13 digits)
    [InlineData("123456", false)] // 6 digits - invalid
    [InlineData("HRB 12345", true)] // Valid Handelsregisternummer
    [InlineData("HRA 12345", true)] // Valid Handelsregisternummer
    public void Germany_CountryDispatch_ValidatesCorrectly(string? input, bool expectedIsValid)
    {
        var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber(input, "DE");
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("1234567890123", true)] // 13 digits - valid Steuernummer
    [InlineData("123456789012", false)] // 12 digits - invalid
    [InlineData("12345678901234", false)] // 14 digits - invalid
    [InlineData("12345", false)] // 5 digits - invalid (not HRB and not 13 digits)
    [InlineData("123456", false)] // 6 digits - invalid
    [InlineData("HRB 12345", true)] // Valid Handelsregisternummer
    [InlineData("HRA 12345", true)] // Valid Handelsregisternummer
    public void Germany_GlobalValidator_ValidatesCorrectly(string? input, bool expectedIsValid)
    {
        var validator = new GlobalEnterpriseValidator();
        var result = validator.Validate(input, "DE");
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("HRB 12345", true)]
    [InlineData("HRA 12345", true)]
    [InlineData("PR 12345", true)]
    [InlineData("GNR 12345", true)]
    [InlineData("VR 12345", true)]
    [InlineData("HRB12345", true)] // Normalized check
    [InlineData("HRA12345", true)]
    [InlineData("hrb 12345", true)] // Case insensitive
    [InlineData("HRB 123", true)]
    [InlineData("HRB 123456789", true)] // Max 9 digits
    [InlineData("HRB 1234567890", false)] // 10 digits - too long
    [InlineData("HRX 12345", false)] // Invalid prefix
    [InlineData("HRB", false)] // Missing digits
    [InlineData("12345", false)] // Missing prefix
    public void Handelsregisternummer_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = GermanyHandelsregisternummerValidator.ValidateHandelsregisternummer(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Handelsregisternummer_Format_ReturnsFormattedString()
    {
        Assert.Equal("HRB 12345", GermanyHandelsregisternummerValidator.Format("HRB12345"));
        Assert.Equal("HRA 12345", GermanyHandelsregisternummerValidator.Format("hra 12345"));
        Assert.Equal("PR 12345", GermanyHandelsregisternummerValidator.Format("PR12345"));
        Assert.Equal("VR 12345", GermanyHandelsregisternummerValidator.Format("VR12345"));
        Assert.Equal("GNR 12345", GermanyHandelsregisternummerValidator.Format("GNR12345"));
    }
}
