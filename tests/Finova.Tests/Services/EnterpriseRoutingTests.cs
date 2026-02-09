using Finova.Core.Common;
using Finova.Services;
using Finova.Services.Asia;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services;

public class EnterpriseRoutingTests
{
    private readonly GlobalEnterpriseValidator _globalValidator = new();

    [Fact]
    public void SouthKorea_RoutedThroughGlobalEnterprise()
    {
        // Valid South Korean BRN: 1234567891
        var result = _globalValidator.Validate("1234567891", "KR");
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void SouthKorea_RoutedThroughAsiaTaxId()
    {
        var result = AsiaTaxIdValidator.Validate("1234567891", "KR");
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Philippines_RoutedThroughGlobalEnterprise()
    {
        // Valid Philippine TIN: 9 digits
        var result = _globalValidator.Validate("123456789", "PH");
        result.Errors.Should().NotContain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    [Fact]
    public void Philippines_RoutedThroughAsiaTaxId()
    {
        var result = AsiaTaxIdValidator.Validate("123456789", "PH");
        result.Errors.Should().NotContain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    [Fact]
    public void Mexico_RoutedThroughSouthAmericaTaxId()
    {
        // Valid Mexican RFC
        var result = _globalValidator.Validate("XAXX010101000", "MX");
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Russia_IbanRoutedThroughEuropeIban()
    {
        var result = EuropeIbanValidator.ValidateIban("RU0204452560040702810400000000012");
        result.Errors.Should().NotContain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }
}
