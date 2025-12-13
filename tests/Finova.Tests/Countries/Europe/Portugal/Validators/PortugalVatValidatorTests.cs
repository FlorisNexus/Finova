using Finova.Countries.Europe.Portugal.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Portugal.Validators;

public class PortugalVatValidatorTests
{
    [Theory]
    [InlineData("PT502757191")] // Valid Portuguese VAT
    [InlineData("502757191")] // Without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = PortugalVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("PT502757190")] // Invalid checksum
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = PortugalVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("PT502757191", "PT", "502757191")]
    [InlineData("502757191", "PT", "502757191")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new PortugalVatValidator().Parse(vat);
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
        var result = new PortugalVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
