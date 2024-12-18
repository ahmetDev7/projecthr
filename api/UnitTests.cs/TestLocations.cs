using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace projecthr
{
    public class LocationsControllerTests
    {
        [Fact]
        public void Create_Should_Return_Created_Location()
        {
            Mock<LocationsProvider> mockLocationsProvider = new Mock<LocationsProvider>(null, null);
            LocationsController controller = new LocationsController(mockLocationsProvider.Object);

            LocationRequest locationRequest = new LocationRequest
            {
                Row = "B",
                Rack = "2",
                Shelf = "Middle",
                WarehouseId = Guid.NewGuid()
            };

            Location createdLocation = new Location
            {
                Id = Guid.NewGuid(),
                Row = locationRequest.Row,
                Rack = locationRequest.Rack,
                Shelf = locationRequest.Shelf,
                WarehouseId = locationRequest.WarehouseId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            mockLocationsProvider
                .Setup(provider => provider.Create(It.Is<LocationRequest>(lc =>
                    lc.Row == locationRequest.Row &&
                    lc.Rack == locationRequest.Rack &&
                    lc.Shelf == locationRequest.Shelf &&
                    lc.WarehouseId == locationRequest.WarehouseId
                )))
                .Returns(createdLocation);

            IActionResult result = controller.Create(locationRequest);

            OkObjectResult? okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

        }
    }
}
