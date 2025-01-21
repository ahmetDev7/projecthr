namespace api.Tests;

public class UnitTest8
{
    [Fact]
    public void ToUtcOrNull_ReturnsNull_WhenInputIsNull()
    {
        // Arrange
        DateTime? input = null;

        // Act
        DateTime? result = DateUtil.ToUtcOrNull(input);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ToUtcOrNull_ReturnsUtcDateTime_WhenInputIsValidDateTime()
    {
        // Arrange
        DateTime input = new DateTime(2025, 1, 14, 12, 0, 0, DateTimeKind.Unspecified);

        // Act
        DateTime? result = DateUtil.ToUtcOrNull(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(DateTimeKind.Utc, result.Value.Kind);
        Assert.Equal(input, result.Value);
    }

    [Fact]
    public void ToUtcOrNull_MaintainsUtcKind_WhenInputIsAlreadyUtc()
    {
        // Arrange
        DateTime input = new DateTime(2025, 1, 14, 12, 0, 0, DateTimeKind.Utc);

        // Act
        DateTime? result = DateUtil.ToUtcOrNull(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(DateTimeKind.Utc, result.Value.Kind);
        Assert.Equal(input, result.Value);
    }
}
