using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace projecthr
{
    public class LocationUnitTests
    {
        [Fact]
        public void Create_test_location()
        {
            Mock<LocationsProvider> mockLocationsProvider = new Mock<LocationsProvider>(null, null);

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

            LocationsController controller = new LocationsController(mockLocationsProvider.Object);

            IActionResult result = controller.Create(locationRequest);

            OkObjectResult? okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var response = okResult.Value as dynamic;

            LocationResponse? locationResponse = response?.new_location as LocationResponse;

            locationResponse.Should().NotBeNull();
            locationResponse!.Id.Should().Be(createdLocation.Id);
            locationResponse.Row.Should().Be(createdLocation.Row);
            locationResponse.Rack.Should().Be(createdLocation.Rack);
            locationResponse.Shelf.Should().Be(createdLocation.Shelf);
            locationResponse.WarehouseId.Should().Be(createdLocation.WarehouseId);

            mockLocationsProvider.Verify(provider => provider.Create(It.Is<LocationRequest>(lr =>
                lr.Row == locationRequest.Row &&
                lr.Rack == locationRequest.Rack &&
                lr.Shelf == locationRequest.Shelf &&
                lr.WarehouseId == locationRequest.WarehouseId
            )), Times.Once);
        }
    }
}
