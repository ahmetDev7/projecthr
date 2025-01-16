using api.Utils.Number;
namespace api.Tests;

public class UnitTest3
{
    [Fact]
    public void EnsureNonNegative_ReturnsZero_ForNegativeNumbers()
    {
        // Arrange
        int input = -5;

        // Act
        int result = NumberUtil.EnsureNonNegative(input);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void EnsureNonNegative_ReturnsSameValue_ForPositiveNumbers()
    {
        // Arrange
        int input = 10;

        // Act
        int result = NumberUtil.EnsureNonNegative(input);

        // Assert
        Assert.Equal(10, result);
    }
}
