using Utils.Number;
namespace api.Tests;

public class UnitTest6
{
    [Fact]
    public void EnsureNonNegativeWithTwoDecimals_ReturnsZero_ForNegativeDecimal()
    {
        // Arrange
        decimal input = -1.2345m;

        // Act
        decimal result = NumberUtil.EnsureNonNegativeWithTwoDecimals(input);

        // Assert
        Assert.Equal(0m, result);
    }

    [Fact]
    public void EnsureNonNegativeWithTwoDecimals_RoundsToTwoDecimals_ForPositiveDecimal()
    {
        // Arrange
        decimal input = 1.23456m;

        // Act
        decimal result = NumberUtil.EnsureNonNegativeWithTwoDecimals(input);

        // Assert
        Assert.Equal(1.23m, result);
    }
}
