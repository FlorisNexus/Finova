using Finova.Core.Common;
using Finova.Countries.Asia.Singapore.Validators;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.Singapore;

public class SingaporeNricValidatorTests
{
    private readonly SingaporeNricValidator _validator = new();

    [Theory]
    [InlineData("S1234567D")]
    [InlineData("T1234567J")]
    [InlineData("F1234567N")]
    [InlineData("G1234567X")]
    [InlineData("M1234567K")]
    public void Validate_ValidNric_ReturnsSuccess(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("S1234567")]   // Too short (8 chars)
    [InlineData("S1234567DA")] // Too long (10 chars)
    public void Validate_InvalidLength_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("S1234567A")] // Invalid checksum (S1234567 map is D)
    [InlineData("A1234567D")] // Invalid prefix
    public void Validate_InvalidFormat_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Match(c => c == ValidationErrorCode.InvalidChecksum || c == ValidationErrorCode.InvalidFormat);
    }

    [Fact]
    public void Validate_RoutedThroughGlobalIdentityValidator()
    {
        var result = GlobalIdentityValidator.ValidateNationalId("SG", "S1234567D");
        result.IsValid.Should().BeTrue();
    }
}
