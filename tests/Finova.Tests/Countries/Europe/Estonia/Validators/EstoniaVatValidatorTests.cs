using Finova.Countries.Europe.Estonia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Estonia.Validators;

public class EstoniaVatValidatorTests
{
    [Theory]
    [InlineData("EE100931558")] // Valid Estonian VAT
    [InlineData("100931558")] // Without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = EstoniaVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("EE100931550")] // Invalid checksum
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = EstoniaVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("EE100931558", "EE", "100931558")]
    [InlineData("100931558", "EE", "100931558")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new EstoniaVatValidator().Parse(vat);
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
        var result = new EstoniaVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
