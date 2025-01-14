namespace api.Tests;

public class UnitTest7
{
    [Fact]
    public void ContainsDuplicateId_ReturnsTrue_WhenDuplicateIdsExist()
    {
        // Arrange
        List<Guid?> ids = new()
        {
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        ids.Add(ids[2]); // Voeg een duplicaat toe.

        // Act
        bool result = CollectionUtil.ContainsDuplicateId(ids);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsDuplicateId_ReturnsFalse_WhenAllIdsAreUnique()
    {
        // Arrange
        List<Guid?> ids = new()
        {
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        // Act
        bool result = CollectionUtil.ContainsDuplicateId(ids);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsDuplicateId_ReturnsTrue_WhenNullIdIsPresent()
    {
        // Arrange
        List<Guid?> ids = new()
        {
            Guid.NewGuid(),
            null,
            Guid.NewGuid()
        };

        // Act
        bool result = CollectionUtil.ContainsDuplicateId(ids);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsDuplicateId_ReturnsFalse_WhenListIsEmpty()
    {
        // Arrange
        List<Guid?> ids = new();

        // Act
        bool result = CollectionUtil.ContainsDuplicateId(ids);

        // Assert
        Assert.False(result);
    }
}
