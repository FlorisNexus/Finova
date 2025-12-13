using Finova.Countries.Europe.Lithuania.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Lithuania.Validators;

public class LithuaniaVatValidatorTests
{
    [Theory]
    [InlineData("LT100001911")] // Valid 9-digit Lithuanian VAT
    [InlineData("100001911")] // Without prefix
    [InlineData("LT123456786")] // Another valid 9-digit
    [InlineData("LT119511515")] // Another valid 9-digit
    public void Validate_WithValid9DigitVat_ReturnsSuccess(string vat)
    {
        var result = LithuaniaVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LT123456789011")] // Valid 12-digit VAT
    [InlineData("123456789011")] // Without prefix
    [InlineData("LT100000000001")] // Another valid 12-digit
    public void Validate_WithValid12DigitVat_ReturnsSuccess(string vat)
    {
        var result = LithuaniaVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LT 1000 0191 1")] // With spaces
    [InlineData(" LT100001911 ")] // With whitespace
    public void Validate_WithFormattedVat_ReturnsSuccess(string vat)
    {
        var result = LithuaniaVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("LT10000191")] // Too short (8 digits)
    [InlineData("LT1000019190")] // 10 digits - invalid length
    [InlineData("LT10000191X")] // Contains letter in number
    [InlineData("XX100001911")] // Wrong prefix
    [InlineData("LT100001910")] // Invalid checksum
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = LithuaniaVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("LT100001911", "LT", "100001911")]
    [InlineData("100001911", "LT", "100001911")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new LithuaniaVatValidator().Parse(vat);
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be(expectedCountryCode);
        result.VatNumber.Should().Be(expectedVatNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("LT100001910")] // Invalid checksum
    public void Parse_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = new LithuaniaVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
