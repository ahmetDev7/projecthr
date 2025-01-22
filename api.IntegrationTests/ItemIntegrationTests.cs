using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DTO.Item;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class ItemIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Items";
        private readonly string adminKey = ApiKeyLoader.LoadApiKeyFromJson(".env.json", "API_ADMIN");

        public ItemIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminKey);
        }

        [Fact]
        public async Task GetItem_ItemExist_ReturnsSuccesWithItem()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            var result = await response.Content.ReadFromJsonAsync<List<Item>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(7);
        }

        [Fact]
        public async Task GetSingleItem_ReturnsSuccesWithItem()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/ab868b64-2a27-451a-be78-105e824547be");
            var result = await response.Content.ReadFromJsonAsync<Item>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        // Get Inventories
        [Fact]
        public async Task GetItemSingle_ReturnsSuccesWithRelatedInventory()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/629b77d6-0256-4d35-a47a-53369042e645/inventories");
            var result = await response.Content.ReadFromJsonAsync<InventoryResponse>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        // Get Inventories totals
        [Fact]
        public async Task GetItemSingle_ReturnsSuccesWithRelatedInventoryTotals()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/629b77d6-0256-4d35-a47a-53369042e645/inventories/totals");
            var result = await response.Content.ReadFromJsonAsync<object>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
        }

        // Create
        [Fact]
        public async Task CreateItem_CreatesNewItem()
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, new ItemRequest()
            {
                Code = "PRD-001",
                Description = "High-Quality Office Chair",
                ShortDescription = "Ergonomic office chair with lumbar support",
                UpcCode = "789456123012",
                ModelNumber = "CH-ERG-2025",
                CommodityCode = "94017100",
                UnitPurchaseQuantity = 50,
                UnitOrderQuantity = 10,
                PackOrderQuantity = 5,
                SupplierReferenceCode = "SUP-ERG-001",
                SupplierPartNumber = "ERG2025CH",
                SupplierId = Guid.Parse("87155264-b98e-4d7a-bb9a-fd1c8eb070b8"),
                CreatedBy = "admin",
            });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // Update
        [Fact]
        public async Task UpdateItem_UpdateItem()
        {
            var updateResponse = await _httpClient.PutAsJsonAsync(_baseUrl + "/629b77d6-0256-4d35-a47a-53369042e645", new ItemRequest()
            {
                Code = "PRD-001",
                Description = "High-Quality Office Chair with lumbar support",
                ShortDescription = "Ergonomic office chair",
                UpcCode = "789456123012",
                ModelNumber = "CH-ERG-2025",
                CommodityCode = "94017100",
                UnitPurchaseQuantity = 100,
                UnitOrderQuantity = 80,
                PackOrderQuantity = 75,
                SupplierReferenceCode = "SUP-ERG-001",
                SupplierPartNumber = "ERG2025CH",
                SupplierId = Guid.Parse("87155264-b98e-4d7a-bb9a-fd1c8eb070b8"),
                CreatedBy = "Warehouse Manager",
            });
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // Delete
        [Fact]
        public async Task DeleteItem_DeleteExistingItem_CheckIfItem()
        {
            string url = _baseUrl + "/629b77d6-0256-4d35-a47a-53369042e651";
            var deleteResponse = await _httpClient.DeleteAsync(url);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync(url);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
