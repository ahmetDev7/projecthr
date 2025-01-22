using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class WarehousesIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Warehouses";

        private readonly string adminKey = ApiKeyLoader.LoadApiKeyFromJson(".env.json", "API_ADMIN");

        public WarehousesIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminKey);
        }

        [Fact]
        public async Task GetWarehouses_WarehouseExist_ReturnsSuccesWithWarehouses()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            var result = await response.Content.ReadFromJsonAsync<List<Warehouse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetSingleWarehouse_ReturnsSuccesWithWarehouse()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/78b5c277-8784-4eb6-ac7d-a1f07dab6e49");
            var result = await response.Content.ReadFromJsonAsync<Warehouse>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetSingleWarehouse_ReturnsSuccesWithRelatedLocations()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/78b5c277-8784-4eb6-ac7d-a1f07dab6e49/locations");
            var result = await response.Content.ReadFromJsonAsync<List<LocationResponse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(4);
        }

        [Fact]
        public async Task GetSingleWarehouse_ReturnsSuccesWithRelatedDock()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/78b5c277-8784-4eb6-ac7d-a1f07dab6e49/dock");
            var result = await response.Content.ReadFromJsonAsync<DockResponse>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateWarehouse_CreatesNewWarehouse()
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, new WarehouseRequest()
            {
                Name = "Sales",
                Code = "Jan Frederik",
                ContactIds = [Guid.Parse("88366127-2bb6-4656-ac24-760a27623a07")],
                AddressId = Guid.Parse("2ff2c789-726b-4dee-b026-622b48a61099")
            });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateWarehouse_UpdateWarehouse()
        {
            var updateResponse = await _httpClient.PutAsJsonAsync(_baseUrl + "/78b5c277-8784-4eb6-ac7d-a1f07dab6e49", new WarehouseRequest()
            {
                Name = "Sales",
                Code = "Jan Frederik",
                ContactIds = [Guid.Parse("88366127-2bb6-4656-ac24-760a27623a07"), Guid.Parse("77366127-2bb6-4656-ac24-760a27623a08")],
                AddressId = Guid.Parse("2ff2c789-726b-4dee-b026-622b48a61099")
            });
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteWarehouse_DeleteExistingWarehouse_CheckIfWarehouse()
        {
            string url = _baseUrl + "/832a3030-4297-4e97-ba5e-c563241ec982";
            var deleteResponse = await _httpClient.DeleteAsync(url);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync(url);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetSingleWarehouse_ReturnsNotFoundWithWarehouse()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/758a3484-cedf-426f-b22c-f0aeed23bc37");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
