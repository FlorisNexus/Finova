using Finova.Countries.Europe.Croatia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Croatia.Validators;

public class CroatiaVatValidatorTests
{
    [Theory]
    [InlineData("HR94577403194")] // Valid Croatian VAT
    [InlineData("94577403194")] // Without prefix
    public void Validate_WithValidVat_ReturnsSuccess(string vat)
    {
        var result = CroatiaVatValidator.Validate(vat);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("HR94577403190")] // Invalid checksum
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidVat_ReturnsFailure(string? vat)
    {
        var result = CroatiaVatValidator.Validate(vat);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("HR94577403194", "HR", "94577403194")]
    [InlineData("94577403194", "HR", "94577403194")]
    public void Parse_WithValidVat_ReturnsDetails(string vat, string expectedCountryCode, string expectedVatNumber)
    {
        var result = new CroatiaVatValidator().Parse(vat);
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
        var result = new CroatiaVatValidator().Parse(vat);
        result.Should().BeNull();
    }
}
