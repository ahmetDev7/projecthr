using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class OrdersIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Orders";

        public OrdersIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        // [Fact]
        // public async Task GetOrders_OrdersExist_ReturnsSuccesWithOrders()
        // {
        //     var response = await _httpClient.GetAsync(_baseUrl);
        //     var result = await response.Content.ReadFromJsonAsync<List<Order>>();
        //     response.StatusCode.Should().Be(HttpStatusCode.OK);

        //     result.Should().HaveCount(1);
        // }

        // [Fact]
        // public async Task GetSingleOrder_ReturnsSuccesWithOrder()
        // {
        //     var response = await _httpClient.GetAsync(_baseUrl + "/7ffe0c5e-c188-47a4-9dcf-f3e17c2ff41c");
        //     var result = await response.Content.ReadFromJsonAsync<Order>();
        //     response.StatusCode.Should().Be(HttpStatusCode.OK);

        //     result.Should().NotBeNull();
        // }
    }
}
