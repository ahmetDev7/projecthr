using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class ItemTypesIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/ItemTypes";

        public ItemTypesIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions{
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetItemTypes_ItemTypesExist_ReturnsSuccesWithItemTypes()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            var result = await response.Content.ReadFromJsonAsync<List<ItemType>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetSingleItemType_ReturnsSuccesWithItemType()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/276b1f8f-f695-46f4-9db0-78ec3f358210");
            var result = await response.Content.ReadFromJsonAsync<ItemType>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            result.Should().NotBeNull();
        }
    }
}
