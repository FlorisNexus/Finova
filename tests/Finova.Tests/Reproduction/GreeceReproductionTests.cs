using Finova.Services;
using Finova.Core.Common;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Reproduction;

public class GreeceReproductionTests
{
    [Fact]
    public void EuropeValidator_ShouldSupport_GR()
    {
        // 094019245 is a valid AFM
        var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber("094019245", "GR");

        // If it fails, we want to see the error
        if (!result.IsValid)
        {
            // This will fail with the message if invalid
            result.IsValid.Should().BeTrue($"Error: {string.Join(", ", result.Errors.Select(e => e.Message))}");
        }
    }

    [Fact]
    public void GlobalValidator_ShouldSupport_GR()
    {
        var validator = new GlobalEnterpriseValidator();
        var result = validator.Validate("094019245", "GR");

        if (!result.IsValid)
        {
            result.IsValid.Should().BeTrue($"Error: {string.Join(", ", result.Errors.Select(e => e.Message))}");
        }
    }

    [Fact]
    public void GlobalValidator_ShouldSupport_GR_WithSpace()
    {
        var validator = new GlobalEnterpriseValidator();
        // This is the suspected cause: "GR " instead of "GR"
        // If this fails, we know we need to Trim() the input
        var result = validator.Validate("094019245", "GR ");

        result.IsValid.Should().BeTrue("Should handle trimmed country code");
    }
}
