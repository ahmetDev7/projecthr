using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class ItemLinesIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/ItemLines";
        private readonly string adminKey = ApiKeyLoader.LoadApiKeyFromJson(".env.json", "API_ADMIN");

        public ItemLinesIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminKey);
        }

        [Fact]
        public async Task GetAllItemLines_ItemLinesExist_ReturnsSuccesWithItemLines()
        {
            var response = await _httpClient.GetAsync(_baseUrl);

            var result = await response.Content.ReadFromJsonAsync<List<ItemLine>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetSingleItemLine_ReturnsSuccesWithItemLine()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/dac7430d-c2c9-48f3-ad74-f443649c0c43");

            var result = await response.Content.ReadFromJsonAsync<ItemLine>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllItemsSpecificSingle_ReturnsSuccesWithRelatedItems()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/dac7430d-c2c9-48f3-ad74-f443649c0c43" + "/items");

            var result = await response.Content.ReadFromJsonAsync<List<Item>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(2);
        }


        [Fact]
        public async Task CreateItemLine_CreatesNewItemLine()
        {
            var newItemLine = new ItemLine
            {
                Name = "Item Line 4",
                Description = "Description Item Line 4"
            };

            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newItemLine);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateItemLine_UpdateNewItemLine()
        {
            var updatedItemLine = new ItemLine
            {
                Name = "Updated Item Line 2",
                Description = "Updated Description Item Line 2"
            };

            var updateResponse = await _httpClient.PutAsJsonAsync(_baseUrl + "/" + "5555260d-e59d-442d-8dfc-305d53a8e4f5", updatedItemLine);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteItemLine_DeleteExistingItemLine_CheckIfDeleted()
        {
            // Item Line 3 delete
            var deleteResponse = await _httpClient.DeleteAsync(_baseUrl + "/" + "1a460afb-7922-4eec-a633-cebea9b9f3fb");

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync(_baseUrl + "/" + "1a460afb-7922-4eec-a633-cebea9b9f3fb");

            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetSingleItemLine_ReturnsNotFoundWithItemLine()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/758a3484-cedf-426f-b22c-f0aeed23bc37");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
