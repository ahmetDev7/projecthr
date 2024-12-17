using System;
using System.Net.Http;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;

public class TestLocation
{
    private readonly HttpClient _client;

    public TestLocation()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
    }

    [Fact]
    public async Task TestCreateLocation()
    {
        Guid warehouseId = Guid.Parse("5faa39e7-c9f5-48cf-b44b-720a9d22d0f9");
        var newLocation = new LocationResponse
        {
            Id = Guid.NewGuid(),
            Row = "MYTEST",
            Rack = "TEST LOCATION",
            Shelf = "2",
            WarehouseId = warehouseId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var content = new StringContent(JsonConvert.SerializeObject(newLocation), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/locations", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}