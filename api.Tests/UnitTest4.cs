namespace api.Tests;

public class UnitTest4
{
    [Fact]
    public void EnsureNonNegativeWithFourDecimals_ReturnsZero_ForNegativeDecimal()
    {
        // Arrange
        decimal input = -5.6789m;

        // Act
        decimal result = NumberUtil.EnsureNonNegativeWithFourDecimals(input);

        // Assert
        Assert.Equal(0m, result);
    }

    [Fact]
    public void EnsureNonNegativeWithFourDecimals_RoundsToFourDecimals_ForPositiveDecimal()
    {
        // Arrange
        decimal input = 5.678945m;

        // Act
        decimal result = NumberUtil.EnsureNonNegativeWithFourDecimals(input);

        // Assert
        Assert.Equal(5.6789m, result);
    }
}
