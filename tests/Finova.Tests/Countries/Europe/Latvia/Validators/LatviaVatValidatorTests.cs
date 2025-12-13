using Finova.Countries.Europe.Latvia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Latvia.Validators;

public class LatviaVatValidatorTests
{
    [Theory]
    [InlineData("LV40003521600")] // Valid Latvian VAT
    [InlineData("40003521600")] // Without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = LatviaVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("LV40003521601")] // Invalid checksum
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = LatviaVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("LV40003521600", "LV", "40003521600")]
    [InlineData("40003521600", "LV", "40003521600")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new LatviaVatValidator().Parse(vat);
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
        var result = new LatviaVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
