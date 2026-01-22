using Finova.Countries.Europe.Germany.Validators;
using Finova.Services;
using Xunit;

namespace Finova.Tests.Reproduction;

public class GermanyEnterpriseReproductionTests
{
    [Fact]
    public void Validate_LongHandelsregisternummer_ShouldBeInvalid()
    {
        // Arrange
        string input = "HRA 12345123456789876545678";

        // Act
        var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber(input, "DE");

        // Assert
        Assert.False(result.IsValid);
        // It should return the Handelsregisternummer error because it has the prefix
        Assert.Contains("Handelsregisternummer", result.Errors[0].Message);
    }

    [Fact]
    public void Validate_SteuernummerWithPrefix_ShouldBeInvalid()
    {
        // Arrange
        string input = "HRA 1234567890123"; // 13 digits but with prefix

        // Act
        var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber(input, "DE");

        // Assert
        Assert.False(result.IsValid);
        // It should still return the Handelsregisternummer error because it has the prefix and is too long for HR
        Assert.Contains("Handelsregisternummer", result.Errors[0].Message);
    }

    [Fact]
    public void Validate_NewPrefixes_ShouldBeValid()
    {
        // Act & Assert
        Assert.True(GermanyHandelsregisternummerValidator.ValidateHandelsregisternummer("PR 12345").IsValid);
        Assert.True(GermanyHandelsregisternummerValidator.ValidateHandelsregisternummer("GnR 12345").IsValid);
        Assert.True(GermanyHandelsregisternummerValidator.ValidateHandelsregisternummer("VR 12345").IsValid);
    }

    [Fact]
    public void Format_NewPrefixes_ShouldHandleSpaceCorrectly()
    {
        // Act & Assert
        Assert.Equal("PR 12345", GermanyHandelsregisternummerValidator.Format("PR12345"));
        Assert.Equal("VR 12345", GermanyHandelsregisternummerValidator.Format("VR12345"));
        Assert.Equal("GNR 12345", GermanyHandelsregisternummerValidator.Format("GnR12345"));
        Assert.Equal("HRA 12345", GermanyHandelsregisternummerValidator.Format("HRA12345"));
    }
}
