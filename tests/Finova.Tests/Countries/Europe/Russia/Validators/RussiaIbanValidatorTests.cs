using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Russia.Validators;

public class RussiaIbanValidatorTests
{
    [Theory]
    [InlineData("RU5104452560040702810400000000012")]
    public void Validate_ValidRussianIban_ReturnsSuccess(string iban)
    {
        var result = EuropeIbanValidator.ValidateIban(iban);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("RU12")] // Too short
    public void Validate_InvalidInput_ReturnsFailure(string? iban)
    {
        var result = EuropeIbanValidator.ValidateIban(iban);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_RussiaRouted_ThroughEuropeIbanValidator()
    {
        // Verify RU is actually routed (not returning UnsupportedCountry)
        var result = EuropeIbanValidator.ValidateIban("RU5104452560040702810400000000012");
        result.Errors.Should().NotContain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }
}
