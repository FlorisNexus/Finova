using Finova.Countries.Europe.Ireland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Ireland.Validators;

public class IrelandVatValidatorTests
{
    [Theory]
    [InlineData("IE6433435F")] // Valid Irish VAT
    [InlineData("6433435F")] // Without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = IrelandVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("IE6433435A")] // Invalid checksum
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = IrelandVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("IE6433435F", "IE", "6433435F")]
    [InlineData("6433435F", "IE", "6433435F")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new IrelandVatValidator().Parse(vat);
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
        result.CountryCode.Should().Be(expectedCountryCode);
        result.VatNumber.Should().Be(expectedVatNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Parse_WithInvalidVat_ReturnsNull(string? vat)
    {
        var result = new IrelandVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
