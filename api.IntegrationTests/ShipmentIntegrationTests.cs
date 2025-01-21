using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class ShipmentIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Shipments";
        private readonly string adminKey = ApiKeyLoader.LoadApiKeyFromJson(".env.json", "API_ADMIN");

        public ShipmentIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminKey);
        }

        [Fact]
        public async Task GetShipments_ShipmentsExist_ReturnsSuccesWithShipments()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            var result = await response.Content.ReadFromJsonAsync<List<Shipment>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetSingleShipment_ReturnsSuccesWithShipment()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/9f0f1aaa-7cfb-48ed-bcfc-7f33c62ecf74");
            var result = await response.Content.ReadFromJsonAsync<Shipment>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }
    }
}
