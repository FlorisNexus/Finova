using Finova.Countries.Europe.Croatia.Validators;
using Finova.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Croatia.Validators;

public class CroatiaNationalIdValidatorTests
{
    private readonly CroatiaOibValidator _validator = new();

    [Theory]
    [InlineData("81793146560")] // Valid OIB
    public void Validate_ValidOib_ReturnsSuccess(string id)
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
    }

    [Fact]
    public void Validate_RoutedThroughGlobalIdentityValidator()
    {
        var result = GlobalIdentityValidator.ValidateNationalId("HR", "81793146560");
        result.IsValid.Should().BeTrue();
    }
}
