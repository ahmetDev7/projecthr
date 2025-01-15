using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class ContactsIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Addresses";
        private readonly string adminKey = ApiKeyLoader.LoadApiKeyFromJson(".env.json", "API_ADMIN");

        public ContactsIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminKey);
        }

        [Fact]
        public async Task GetAllContacts_ContactsExist_ReturnsSuccesWithContacts()
        {
            var response = await _httpClient.GetAsync(_baseUrl);

            var result = await response.Content.ReadFromJsonAsync<List<Contact>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetSingleContacts_ReturnsSuccesWithContacts()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/cefc9e60-7d37-41f5-b3c8-3144894f207e");

            var result = await response.Content.ReadFromJsonAsync<Contact>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }
    }
}
