using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Newtonsoft.Json.Linq;

namespace api.IntegrationTests
{
    public class ItemTypesIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ItemTypesIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ItemTypes_ReturnsOkAndNonEmptyList()
        {
            // Arrange
            var url = "/api/ItemTypes";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonArray = JArray.Parse(responseBody);

            Assert.True(jsonArray.Count > 0, "Expected non-empty list of item types.");
        }
    }
}
