using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class ItemGroupsIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/ItemGroups";
        private readonly string adminKey = ApiKeyLoader.LoadApiKeyFromJson(".env.json", "API_ADMIN");

        public ItemGroupsIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminKey);
        }

        [Fact]
        public async Task GetAllItemGroups_ItemGroupsExist_ReturnsSuccesWithItemTypes()
        {
            var response = await _httpClient.GetAsync(_baseUrl);

            var result = await response.Content.ReadFromJsonAsync<List<ItemType>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetSingleItemGroup_ReturnsSuccesWithItemGroup()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/4604084f-a55f-484f-8707-feae90c72fcd");

            var result = await response.Content.ReadFromJsonAsync<ItemGroup>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllItemsSpecificSingle_ReturnsSuccesWithRelatedItems()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/4604084f-a55f-484f-8707-feae90c72fcd" + "/items");
            var result = await response.Content.ReadFromJsonAsync<List<Item>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(2);
        }


        [Fact]
        public async Task CreateItemGroup_CreatesNewItemGroup()
        {
            var newItemGroup = new ItemGroup
            {
                Name = "Item Group 4",
                Description = "Description Item Group 4"
            };

            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newItemGroup);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateItemGroup_UpdateNewItemGroup()
        {
            var updatedItemGroup = new ItemGroup
            {
                Name = "Updated Item Group 2",
                Description = "Updated Description Item Group 2"
            };

            var updateResponse = await _httpClient.PutAsJsonAsync(_baseUrl + "/" + "4428de87-dd7f-4879-823b-ec9f97e50add", updatedItemGroup);

            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteItemGroup_DeleteExistingItemGroup_CheckIfDeleted()
        {
            // Item Group 3 delete
            var deleteResponse = await _httpClient.DeleteAsync(_baseUrl + "/" + "00654a9f-83c1-49db-acb0-6d908c0520fc");

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync(_baseUrl + "/" + "00654a9f-83c1-49db-acb0-6d908c0520fc");

            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
