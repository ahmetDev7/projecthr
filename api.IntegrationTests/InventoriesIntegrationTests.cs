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

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetSingleInventory_ReturnSuccessWithInventory()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/722eec5c-9de0-4993-8aea-3b473ec30d22");

            var result = await response.Content.ReadFromJsonAsync<Inventory>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        // Create
        [Fact]
        public async Task CreateInventory_CreatesNewInventory()
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, new InventoryRequest()
            {
                Description = "Some item Inventory",
                ItemReference = "DXV-247-2RG",
                ItemId = Guid.Parse("a0def768-b4b2-484d-80f4-3268719ccc3d"),
            });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // Update
        [Fact]
        public async Task UpdateInventory_UpdateInventory()
        {
            var updateResponse = await _httpClient.PutAsJsonAsync(_baseUrl + "/722eec5c-9de0-4993-8aea-3b473ec30d22", new InventoryRequest()
            {
                Description = "Some item Inventory updated",
                ItemReference = "DXV-247-2RG updated",
                ItemId = Guid.Parse("a0def768-b4b2-484d-80f4-3268719ccc4d"),
                Locations = new List<InventoryLocationRR>{
                        new InventoryLocationRR() {
                            LocationId = Guid.Parse("91629396-1d08-4f77-9049-c49216870112"),
                            OnHand = 70
                        }
                    }
            });
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // Delete
        [Fact]
        public async Task DeleteInventory_DeleteExistingInventory_CheckIfInventory()
        {
            string url = _baseUrl + "/722eec5c-9de0-4993-8aea-3b473ec30d79";
            var deleteResponse = await _httpClient.DeleteAsync(url);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync(url);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
