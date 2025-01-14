using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class WarehousesIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Warehouses";

        public WarehousesIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetWarehouses_WarehouseExist_ReturnsSuccesWithWarehouses()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            var result = await response.Content.ReadFromJsonAsync<List<Warehouse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetSingleWarehouse_ReturnsSuccesWithWarehouse()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/78b5c277-8784-4eb6-ac7d-a1f07dab6e49");
            var result = await response.Content.ReadFromJsonAsync<Warehouse>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }
    }
}
