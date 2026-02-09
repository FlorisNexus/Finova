using Finova.Core.Common;
using Finova.Countries.Europe.Iceland.Validators;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Iceland;

public class IcelandNationalIdValidatorTests
{
    private readonly IcelandKennitalaValidator _validator = new();

    [Theory]
    [InlineData("1501893069")] // Valid Kennitala: 15 Jan 1989, check=6, century=9
    public void Validate_ValidKennitala_ReturnsSuccess(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("150189300")] // Too short
    public void Validate_InvalidLength_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("1501893000")] // Invalid checksum
    public void Validate_InvalidChecksum_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidChecksum);
    }

    [Fact]
    public void Validate_RoutedThroughGlobalIdentityValidator()
    {
        var result = GlobalIdentityValidator.ValidateNationalId("IS", "1501893069");
        result.IsValid.Should().BeTrue();
    }
}
