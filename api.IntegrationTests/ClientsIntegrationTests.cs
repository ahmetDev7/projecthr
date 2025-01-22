using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DTO.Client;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class ClientsIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Clients";

        private readonly string adminKey = ApiKeyLoader.LoadApiKeyFromJson(".env.json", "API_ADMIN");

        public ClientsIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminKey);
        }

        [Fact]
        public async Task GetClients_ClientsExist_ReturnsSuccesWithClients()
        {
            var response = await _httpClient.GetAsync(_baseUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<Client>>();

            result.Should().HaveCount(4);
        }


        [Fact]
        public async Task GetSingleClient_ReturnsSuccesWithClient()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/8f568644-4d30-4658-ab68-c80d0636ba8f");

            var result = await response.Content.ReadFromJsonAsync<Client>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllOrdersSpecificSingle_ReturnsSuccesWithRelatedOrders()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/5adfa1e2-f6ee-4ce2-a1ea-95e8a990a4f3" + "/orders");
            var result = await response.Content.ReadFromJsonAsync<List<Order>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task CreateClient_CreatesNewClient()
        {
            var newClient = new ClientRequest
            {
                Name = "Client 4",
                ContactId = Guid.Parse("88366127-2bb6-4656-ac24-760a27623a07"),
                AddressId = Guid.Parse("cefc9e60-7d37-41f5-b3c8-3144894f207e")
            };

            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newClient);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateClient_UpdatesClient()
        {
            var updatedClient = new ClientRequest
            {
                Name = "Updated Client 2",
                ContactId = Guid.Parse("88366127-2bb6-4656-ac24-760a27623a07"),
                AddressId = Guid.Parse("cefc9e60-7d37-41f5-b3c8-3144894f207e")
            };

            var updateResponse = await _httpClient.PutAsJsonAsync(_baseUrl + "/" + "5adfa1e2-f6ee-4ce2-a1ea-95e8a990a4f3", updatedClient);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteClient_DeleteExistingClient_CheckIfDeleted()
        {
            // Client 3 delete
            var deleteResponse = await _httpClient.DeleteAsync(_baseUrl + "/" + "39fbea38-bb4b-49db-b9dc-fe1aec620441");

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync(_baseUrl + "/" + "39fbea38-bb4b-49db-b9dc-fe1aec620441");

            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetSingleClient_ReturnsNotFoundWithClient()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/758a3484-cedf-426f-b22c-f0aeed23bc37");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
