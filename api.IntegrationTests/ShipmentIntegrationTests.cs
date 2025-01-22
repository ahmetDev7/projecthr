using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DTO.Item;
using DTO.Order;
using DTO.Shipment;
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

            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetSingleShipment_ReturnsSuccesWithShipment()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/9f0f1aaa-7cfb-48ed-bcfc-7f33c62ecf74");
            var result = await response.Content.ReadFromJsonAsync<Shipment>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        // GET /api/Shipments/{shipmentId}/orders
        [Fact]
        public async Task GetShipmentOrders_ReturnsSuccessWithOrders()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/9f0f1aaa-7cfb-48ed-bcfc-7f33c62ecf74/orders");
            var result = await response.Content.ReadFromJsonAsync<List<OrderResponse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        // GET /api/Shipments/{shipmentId}/items
        [Fact]
        public async Task GetShipmentItems_ReturnsSuccessWithItems()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/9f0f1aaa-7cfb-48ed-bcfc-7f33c62ecf74/items");
            var result = await response.Content.ReadFromJsonAsync<List<ItemResponse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        // POST Create
        [Fact]
        public async Task CreateShipment_CreatesNewShipmentSuccess()
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, new ShipmentRequest()
            {
                ShipmentType = ShipmentType.O,
                CarrierCode = "010101",
                ServiceCode = "304402",
                PaymentType = PaymentType.Automatic,
                TransferMode = TransferMode.Air,
                TotalPackageCount = 1,
                TotalPackageWeight = 12,
                Items = new List<ShipmentItemRR>(){
                    new ShipmentItemRR() {
                        ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                        Amount = 49
                    }
                }
            });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // PUT Update
        [Fact]
        public async Task UpdateShipment_UpdateShipmentSuccess()
        {
            var response = await _httpClient.PutAsJsonAsync(_baseUrl + "/9f0f1aaa-7cfb-48ed-bcfc-7f33c62ecf74", new ShipmentRequest()
            {
                ShipmentType = ShipmentType.O,
                CarrierCode = "9238047",
                ServiceCode = "43579",
                PaymentType = PaymentType.Manual,
                TransferMode = TransferMode.Ground,
                ShipmentStatus = ShipmentStatus.Pending,
                TotalPackageCount = 2,
                TotalPackageWeight = 23,
                Items = new List<ShipmentItemRR>(){
                    new ShipmentItemRR() {
                        ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                        Amount = 89
                    }
                }
            });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //  PUT Commit
        [Fact]
        public async Task CommitShipment_CommitShipmentSuccess()
        {
            var response = await _httpClient.PutAsync(_baseUrl + "/67833cb8-9d20-4c13-922b-26c354a97a9f/commit", null);

            var result = await response.Content.ReadFromJsonAsync<ShipmentResponse>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
        }

        //  PUT /api/Shipments/{shipmentId}/orders
        [Fact]
        public async Task UpdateShipmentOrders_UpdateShipmentOrdersSuccess()
        {
            var response = await _httpClient.PutAsJsonAsync(_baseUrl + "/9f0f1aaa-7cfb-48ed-bcfc-7f33c62ecf74/orders",
            new List<Guid?>()
            {
            });

            var result = await response.Content.ReadFromJsonAsync<ShipmentResponse>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
        }

        //  PUT /api/Shipments/{shipmentId}/items
        [Fact]
        public async Task UpdateShipmentItems_UpdateShipmentItemsSuccess()
        {
            var response = await _httpClient.PutAsJsonAsync(_baseUrl + "/9f0f1aaa-7cfb-48ed-bcfc-7f33c62ecf74/items",
            new List<ShipmentItemRR>()
            {
                new ShipmentItemRR(){
                    ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                    Amount = 10
                }
            });

            var result = await response.Content.ReadFromJsonAsync<ShipmentResponse>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
        }


        // Delete
        [Fact]
        public async Task DeleteOrder_DeleteExistingOrder_CheckIfOrder()
        {
            string url = _baseUrl + "/4786f6b5-b2ed-4ed9-9cff-7d5333e4925e";
            var deleteResponse = await _httpClient.DeleteAsync(url);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync(url);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
