using Finova.Core.Common;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Services.Global;

public class GlobalIdentityValidatorTests
{
    [Fact]
    public void ValidateNationalId_ShouldReturnTrue_ForValidChinaId()
    {
        // 11010519491231002X is a valid ID (checksum X)
        var result = GlobalIdentityValidator.ValidateNationalId("CN", "11010519491231002X");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("JP", "111111111118")] // Japan My Number
    [InlineData("SG", "S1234567D")]     // Singapore NRIC
    [InlineData("IL", "300000007")]     // Israel Teudat Zehut
    [InlineData("SA", "1000000008")]    // Saudi Arabia National ID
    [InlineData("AE", "784198412345674")] // UAE Emirates ID
    [InlineData("US", "123-45-6789")]   // USA SSN
    [InlineData("NG", "12345678901")]    // Nigeria NIN
    [InlineData("ZA", "8001015000086")] // South Africa ID
    public void ValidateNationalId_ShouldReturnTrue_ForValidNewlyRoutedIds(string country, string id)
    {
        var result = GlobalIdentityValidator.ValidateNationalId(country, id);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateNationalId_ShouldReturnFalse_ForInvalidCountry()
    {
        var result = GlobalIdentityValidator.ValidateNationalId("XX", "123");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.UnsupportedCountry);
    }

    [Fact]
    public void ValidateTaxId_ShouldReturnTrue_ForValidUsEin()
    {
        // 12-3456789
        var result = GlobalIdentityValidator.ValidateTaxId("US", "12-3456789");
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateTaxId_ShouldReturnTrue_ForValidAustraliaAbn()
    {
        // 51 824 753 556
        var result = GlobalIdentityValidator.ValidateTaxId("AU", "51 824 753 556");
        result.IsValid.Should().BeTrue();
    }
}
