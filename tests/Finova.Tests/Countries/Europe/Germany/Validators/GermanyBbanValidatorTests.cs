using Finova.Core.Common;
using Finova.Countries.Europe.Germany.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Germany.Validators;

public class GermanyBbanValidatorTests
{
    [Theory]
    [InlineData("370400440532013000")] // Valid
    public void Validate_ShouldReturnSuccess_ForValidBban(string bban)
    {
        // Act
        var result = GermanyBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldReturnFailure_ForEmptyInput(string? input)
    {
        // Act
        var result = GermanyBbanValidator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("37040044 0532013099")] // Invalid Checksum (hypothetical, Germany has complex rules but basic check)
    [InlineData("ABC")]                 // Invalid Length
    public void Validate_ShouldReturnFailure_ForInvalidBban(string bban)
    {
        // Act
        var result = GermanyBbanValidator.Validate(bban);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
