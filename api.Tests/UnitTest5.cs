using api.Utils.Number;
namespace api.Tests;

public class UnitTest5
{
    [Fact]
    public void MinimumInt_ReturnsMinimum_WhenNumberIsSmallerThanMinimum()
    {
        // Arrange
        int input = 3;
        int minimum = 10;

        // Act
        int result = NumberUtil.MinimumInt(input, minimum);

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void MinimumInt_ReturnsNumber_WhenNumberIsGreaterThanMinimum()
    {
        // Arrange
        int input = 15;
        int minimum = 10;

        // Act
        int result = NumberUtil.MinimumInt(input, minimum);

        // Assert
        Assert.Equal(15, result);
    }
}
