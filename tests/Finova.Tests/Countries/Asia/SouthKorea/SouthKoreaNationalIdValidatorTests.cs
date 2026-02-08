using Finova.Core.Common;
using Finova.Countries.Asia.SouthKorea.Validators;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.SouthKorea;

public class SouthKoreaNationalIdValidatorTests
{
    private readonly SouthKoreaNationalIdValidator _validator = new();

    [Theory]
    [InlineData("8601011234563")] // 1986-01-01, male born 1900s, valid checksum
    public void Validate_ValidRrn_ReturnsSuccess(string id)
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
    [InlineData("860101123456")]   // Too short (12 digits)
    [InlineData("86010112345612")] // Too long (14 digits)
    public void Validate_InvalidLength_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("8601011234560")] // Invalid checksum
    public void Validate_InvalidChecksum_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidChecksum);
    }

    [Fact]
    public void Validate_RoutedThroughGlobalIdentityValidator()
    {
        var result = GlobalIdentityValidator.ValidateNationalId("KR", "8601011234563");
        result.IsValid.Should().BeTrue();
    }
}
