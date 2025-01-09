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
            
            // Seeded 3 Item Types in Seeding.cs
            result.Should().HaveCount(3);
        }
    }
}
