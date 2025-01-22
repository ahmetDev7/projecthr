using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DTO.Item;
using DTO.Order;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class OrdersIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Orders";
        private readonly string adminKey = ApiKeyLoader.LoadApiKeyFromJson(".env.json", "API_ADMIN");

        public OrdersIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminKey);
        }

        [Fact]
        public async Task GetOrders_OrdersExist_ReturnsSuccesWithOrders()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            var result = await response.Content.ReadFromJsonAsync<List<Order>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetSingleOrder_ReturnsSuccesWithOrder()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/7ffe0c5e-c188-47a4-9dcf-f3e17c2ff41c");
            var result = await response.Content.ReadFromJsonAsync<Order>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetSingleOrder_ReturnsSuccesWithOrder_RelatedItems()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/7ffe0c5e-c188-47a4-9dcf-f3e17c2ff41c/items");
            var result = await response.Content.ReadFromJsonAsync<List<ItemResponse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
        }

        // Create
        [Fact]
        public async Task CreateOrder_CreatesNewOrderSuccess()
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, new OrderRequest()
            {
                WarehouseId = Guid.Parse("78b5c277-8784-4eb6-ac7d-a1f07dab6e49"),
                BillToClientId = Guid.Parse("5adfa1e2-f6ee-4ce2-a1ea-95e8a990a4f3"),
                OrderStatus = OrderStatus.Pending,
                OrderItems = new List<OrderItemRequest>() {
                    new OrderItemRequest() {
                        ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                        Amount = 20
                    }
                }
            });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // Update
        [Fact]
        public async Task UpdateOrder_UpdateOrderSuccess()
        {
            // 9eb03425-c377-443b-bf5a-3010de9f4d7d
            var updateResponse = await _httpClient.PutAsJsonAsync(_baseUrl + "/9eb03425-c377-443b-bf5a-3010de9f4d7d", new OrderRequest()
            {
                WarehouseId = Guid.Parse("78b5c277-8784-4eb6-ac7d-a1f07dab6e49"),
                BillToClientId = Guid.Parse("5adfa1e2-f6ee-4ce2-a1ea-95e8a990a4f3"),
                OrderItems = new List<OrderItemRequest>() {
                    new OrderItemRequest() {
                        ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                        Amount = 98
                    }
                }
            });
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        // Commit order
        [Fact]
        public async Task CommitOrder_CommitOrderSuccess()
        {
            var response = await _httpClient.PutAsync(_baseUrl + "/7ffe0c5e-c188-47a4-9dcf-f3e17c2ff41c/commit", null);

            var result = await response.Content.ReadFromJsonAsync<OrderResponse>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
        }

        // Delete
        [Fact]
        public async Task DeleteOrder_DeleteExistingOrder_CheckIfOrder()
        {
            string url = _baseUrl + "/797146d4-d13c-4c00-b89d-fb04980f728d";
            var deleteResponse = await _httpClient.DeleteAsync(url);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync(url);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
