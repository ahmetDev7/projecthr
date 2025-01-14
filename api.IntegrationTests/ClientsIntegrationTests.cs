using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class ClientsIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Clients";

        public ClientsIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetClients_ClientsExist_ReturnsSuccesWithClients()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            var result = await response.Content.ReadFromJsonAsync<List<Client>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetSingleClient_ReturnsSuccesWithClient()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/8f568644-4d30-4658-ab68-c80d0636ba8f");
            var result = await response.Content.ReadFromJsonAsync<Client>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }
    }
}
