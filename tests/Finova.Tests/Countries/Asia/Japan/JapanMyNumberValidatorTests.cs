using Finova.Core.Common;
using Finova.Countries.Asia.Japan.Validators;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.Japan;

public class JapanMyNumberValidatorTests
{
    private readonly JapanMyNumberValidator _validator = new();

    [Theory]
    [InlineData("111111111118")]
    [InlineData("123456789018")]
    public void Validate_ValidMyNumber_ReturnsSuccess(string id)
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
    [InlineData("11111111111")]    // Too short (11 digits)
    [InlineData("1111111111111")]  // Too long (13 digits)
    public void Validate_InvalidLength_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("111111111117")] // Invalid checksum
    public void Validate_InvalidChecksum_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidChecksum);
    }

    [Fact]
    public void Validate_RoutedThroughGlobalIdentityValidator()
    {
        var result = GlobalIdentityValidator.ValidateNationalId("JP", "111111111118");
        result.IsValid.Should().BeTrue();
    }
}
