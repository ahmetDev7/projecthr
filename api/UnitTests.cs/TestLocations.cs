using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace projecthr
{
    public class LocationsControllerTests
    {

        readonly Mock<LocationsProvider> mockLocationsProvider;
        readonly LocationsController controller;

        public LocationsControllerTests()
        {
            mockLocationsProvider = new Mock<LocationsProvider>(null, null);
            controller = new LocationsController(mockLocationsProvider.Object);
        }

        [Fact]
        public void Create_Should_Return_Created_Location()
        {
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

        [Fact]
        public void Update_Should_Return_Updated_Location()
        {
            Location newlocation = new Location
            {
                Id = Guid.NewGuid(),
                Row = "C",
                Rack = "5",
                Shelf = "Top",
                WarehouseId = Guid.NewGuid()
            };

            LocationRequest updatedLocation = new LocationRequest
            {
                Row = newlocation.Row,
                Rack = newlocation.Rack,
                Shelf = "Top",
                WarehouseId = newlocation.WarehouseId,
            };

            mockLocationsProvider
                .Setup(provider => provider.Update(
                    It.Is<Guid>(id => id == newlocation.Id),
                    It.Is<LocationRequest>(lc =>
                        lc.Row == updatedLocation.Row &&
                        lc.Rack == updatedLocation.Rack &&
                        lc.Shelf == updatedLocation.Shelf &&
                        lc.WarehouseId == updatedLocation.WarehouseId)))
                .Returns(newlocation);

            var result = controller.Update(newlocation.Id, updatedLocation);

            OkObjectResult? okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
        }
    }
}
