using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DTO.Contact;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class ContactsIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Contacts";
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

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetSingleContacts_ReturnsSuccesWithContacts()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/88366127-2bb6-4656-ac24-760a27623a07");

            var result = await response.Content.ReadFromJsonAsync<Contact>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateContact_CreatesNewContact()
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, new ContactRequest()
            {
                Name = "Jan Frederik",
                Function = "Sales",
                Phone = "0612345678",
                Email = "jan@cargohub.nl",
            });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateContact_UpdateContact()
        {
            var updateResponse = await _httpClient.PutAsJsonAsync(_baseUrl + "/88366127-2bb6-4656-ac24-760a27623a07", new ContactRequest()
            {
                Name = "Hans Boom",
                Function = "Warehouse manager",
                Phone = "0612345678",
                Email = "hans@cargohub.nl",
            });
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteContact_DeleteExistingContact_CheckIfContact()
        {
            string url = _baseUrl + "/88366127-2bb6-4656-ac24-760a27623a10";
            var deleteResponse = await _httpClient.DeleteAsync(url);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync(url);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
