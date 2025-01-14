using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class LocationIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Locations";

        public LocationIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
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

    }
}