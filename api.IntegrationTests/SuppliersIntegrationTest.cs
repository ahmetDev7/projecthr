using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DTO.Item;
using DTO.Supplier;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class SuppliersIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/Suppliers";
        private readonly string adminKey = ApiKeyLoader.LoadApiKeyFromJson(".env.json", "API_ADMIN");

        public SuppliersIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminKey);
        }

        [Fact]
        public async Task GetSuppliers_SuppliersExist_ReturnsSuccesWithSuppliers()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            var result = await response.Content.ReadFromJsonAsync<List<Supplier>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().HaveCount(3);
        }


        [Fact]
        public async Task GetSuppliers_SuppliersExist_ReturnsSuccesRelatedItems()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/87155264-b98e-4d7a-bb9a-fd1c8eb070b8/items");
            var result = await response.Content.ReadFromJsonAsync<List<ItemResponse>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetSingleSupplier_ReturnsSuccesWithSupplier()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/87155264-b98e-4d7a-bb9a-fd1c8eb070b8");
            var result = await response.Content.ReadFromJsonAsync<Supplier>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateSupplier_CreatesNewSupplier()
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, new SupplierRequest()
            {
                Name = "Dutch Supplies Co.",
                Code = "SUPPLIER_001",
                Reference = "DSC-456-ZXQ",
                ContactId = Guid.Parse("88366127-2bb6-4656-ac24-760a27623a07"),
                AddressId = Guid.Parse("2ff2c789-726b-4dee-b026-622b48a61099")
            });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateSupplier_UpdateSupplier()
        {
            var updateResponse = await _httpClient.PutAsJsonAsync(_baseUrl + "/87155264-b98e-4d7a-bb9a-fd1c8eb070b8", new SupplierRequest()
            {
                Name = "Econmic inc. B.V",
                Code = "SUPPLIER_001",
                Reference = "WHD-456-ZXQ",
                ContactId = Guid.Parse("88366127-2bb6-4656-ac24-760a27623a07"),
                AddressId = Guid.Parse("cefc9e60-7d37-41f5-b3c8-3144894f207e"),
            });
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteSupplier_DeleteExistingSupplier_CheckIfSupplier()
        {
            string url = _baseUrl + "/1c989e40-9b2e-4cd7-bff2-abf42d977e88";
            var deleteResponse = await _httpClient.DeleteAsync(url);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync(url);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
