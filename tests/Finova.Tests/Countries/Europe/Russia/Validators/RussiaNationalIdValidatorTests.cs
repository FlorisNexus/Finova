using Finova.Core.Common;
using Finova.Countries.Europe.Russia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Russia.Validators;

public class RussiaNationalIdValidatorTests
{
    private readonly RussiaNationalIdValidator _validator = new();

    [Theory]
    [InlineData("500100732259")] // Valid 12-digit individual INN
    [InlineData("7707083893")]   // Valid 10-digit corporate INN
    public void Validate_ValidInn_ReturnsSuccess(string id)
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
    [InlineData("123")]         // Too short
    [InlineData("1234567890123")] // Too long (13 digits)
    public void Validate_InvalidLength_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidLength);
    }

    [Theory]
    [InlineData("7707083890")]   // Valid format, invalid checksum
    [InlineData("500100732250")] // Valid format, invalid checksum
    public void Validate_InvalidChecksum_ReturnsFailure(string id)
    {
        var result = _validator.Validate(id);
        result.IsValid.Should().BeFalse();
        result.ErrorCode().Should().Be(ValidationErrorCode.InvalidChecksum);
    }

    [Fact]
    public void Parse_ValidInn_ReturnsNormalized()
    {
        var result = _validator.Parse("7707083893");
        result.Should().NotBeNull();
        result.Should().Be("7707083893");
    }

    [Fact]
    public void Parse_InvalidInn_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
