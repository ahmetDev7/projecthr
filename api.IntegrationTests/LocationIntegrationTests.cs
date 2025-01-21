using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class LocationIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Locations";
        private readonly string adminKey = ApiKeyLoader.LoadApiKeyFromJson(".env.json", "API_ADMIN");

        public LocationIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminKey);
        }

        [Fact]
        public async Task GetLocations_LocationExist_ReturnsSuccesWithLocations()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            var result = await response.Content.ReadFromJsonAsync<List<Location>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetSingleLocation_ReturnsSuccesWithLocation()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/91629396-1d08-4f77-9049-c49216870112");
            var result = await response.Content.ReadFromJsonAsync<Location>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        // create
        [Fact]
        public async Task CreateLocation_CreatesNewLocation()
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, new LocationRequest()
            {
                Row = "10",
                Rack = "10",
                Shelf = "10",
                WarehouseId = Guid.Parse("78b5c277-8784-4eb6-ac7d-a1f07dab6e49"),
            });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        // update
        [Fact]
        public async Task UpdateLocation_UpdateLocation()
        {
            var updateResponse = await _httpClient.PutAsJsonAsync(_baseUrl + "/91629396-1d08-4f77-9049-c49216870112", new LocationRequest()
            {
                Row = "20",
                Rack = "20",
                Shelf = "20",
                WarehouseId = Guid.Parse("78b5c277-8784-4eb6-ac7d-a1f07dab6e49"),
            });
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // delete        
        [Fact]
        public async Task DeleteLocation_DeleteExistingLocation_CheckIfLocation()
        {
            string url = _baseUrl + "/68b1efa8-8cbe-4dae-867f-40384954c5cd";
            var deleteResponse = await _httpClient.DeleteAsync(url);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync(url);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}