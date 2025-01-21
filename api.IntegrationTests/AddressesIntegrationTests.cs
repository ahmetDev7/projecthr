using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DTO.Address;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class AddressesIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Addresses";
        private readonly string adminKey = ApiKeyLoader.LoadApiKeyFromJson(".env.json", "API_ADMIN");

        public AddressesIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminKey);
        }

        [Fact]
        public async Task GetAllAddresses_AddressesExist_ReturnsSuccesWithAddresses()
        {
            var response = await _httpClient.GetAsync(_baseUrl);

            var result = await response.Content.ReadFromJsonAsync<List<Address>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetSingleAddress_ReturnsSuccesWithAddress()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/cefc9e60-7d37-41f5-b3c8-3144894f207e");

            var result = await response.Content.ReadFromJsonAsync<Address>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateAddress_CreatesNewAddress()
        {
            var body = new AddressRequest()
            {
                Street = "Keizersgracht",
                HouseNumber = "456",
                HouseNumberExtension = "B",
                HouseNumberExtensionExtra = "Side Entrance",
                ZipCode = "1015 CX",
                City = "Amsterdam",
                Province = "North Holland",
                CountryCode = "NL"
            };

            var response = await _httpClient.PostAsJsonAsync(_baseUrl, body);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateAddress_UpdateAddress()
        {
            var updateResponse = await _httpClient.PutAsJsonAsync(_baseUrl + "/cefc9e60-7d37-41f5-b3c8-3144894f207e", new AddressRequest()
            {
                Street = "Oranjestraat",
                HouseNumber = "456",
                HouseNumberExtension = "B",
                HouseNumberExtensionExtra = "Side Entrance",
                ZipCode = "1015 CX",
                City = "Amsterdam",
                Province = "North Holland",
                CountryCode = "NL"
            });
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteAddress_DeleteExistingAddress_CheckIfAddress()
        {
            string url = _baseUrl + "/2ff2c789-726b-4dee-b026-622b48a61088";
            var deleteResponse = await _httpClient.DeleteAsync(url);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync(url);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
