using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class InventoriesIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/inventories";
        private readonly string adminKey = ApiKeyLoader.LoadApiKeyFromJson(".env.json", "API_ADMIN");

        public InventoriesIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminKey);
        }

        [Fact]
        //
        public async Task GetAllInventories_InventoriesExists_ReturnSuccessWithInventories()
        {
            var response = await _httpClient.GetAsync(_baseUrl);

            var result = await response.Content.ReadFromJsonAsync<List<Inventory>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetSingleInventory_ReturnSuccessWithInventory()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/722eec5c-9de0-4993-8aea-3b473ec30d22");

            var result = await response.Content.ReadFromJsonAsync<Inventory>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }
    }
}
