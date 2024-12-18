namespace api.Tests;

public class UnitTest1
{
    [Fact]
    public void Test_EnumsToString_ReturnsCorrectString()
    {
        // Arrange
        string expectedResult = "Pending, Processing, Cancelled, Completed";

        // Act
        string result = EnumUtil.EnumsToString<TransferStatus>();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}