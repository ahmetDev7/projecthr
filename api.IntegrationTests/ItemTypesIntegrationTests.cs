using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;

namespace api.IntegrationTests
{
    public class ItemTypesIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "/api/ItemTypes";

        public ItemTypesIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ItemTypes_ReturnsOkAndNonEmptyList()
        {
            var url = _baseUrl;

            var response = await _client.GetAsync(url);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonArray = JArray.Parse(responseBody);

            Assert.True(jsonArray.Count > 0, "Expected non-empty list of item types.");
        }
    }
}
