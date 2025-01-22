using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DTO.Item;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.IntegrationTests
{
    public class TransfersIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/transfers";
        private readonly string adminKey = ApiKeyLoader.LoadApiKeyFromJson(".env.json", "API_ADMIN");

        public TransfersIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminKey);
        }

        [Fact]
        public async Task GetAllTransfers_TransfersExists_ReturnSuccessWithTransfers()
        {
            var response = await _httpClient.GetAsync(_baseUrl);

            var result = await response.Content.ReadFromJsonAsync<List<Transfer>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetSingleTransfer_ReturnSuccessWithTransfer()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/cefc9e60-7d37-41f5-b3c8-3144894f203e");

            var result = await response.Content.ReadFromJsonAsync<Transfer>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        // Get related items from transfer
        [Fact]
        public async Task GetSingleTransfer_ReturnSuccessWithTransfer_RelatedItems()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/cefc9e60-7d37-41f5-b3c8-3144894f203e/items");

            var result = await response.Content.ReadFromJsonAsync<List<ItemResponse>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
        }

        // Create
        [Fact]
        public async Task CreateTransfer_CreatesNewTransfer()
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, new TransferRequestCreate()
            {
                Reference = "TFS-38478",
                TransferFromId = Guid.Parse("91629396-1d08-4f77-9049-c49216870112"),
                TransferToId = Guid.Parse("e6786fad-435b-460f-b6dd-11dd32b3b6a6"),
                Items = new List<TransferItemDTO>()
                    {
                        new TransferItemDTO
                        {
                            ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                            Amount = 34
                        }
                    }
            });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        // Update
        [Fact]
        public async Task UpdateTransfer_UpdateTransfer()
        {
            var updateResponse = await _httpClient.PutAsJsonAsync(_baseUrl + "/cefc9e60-7d37-41f5-b3c8-3144894f203e", new TransferRequestUpdate()
            {
                Reference = "TST-8793",
                TransferFromId = Guid.Parse("91629396-1d08-4f77-9049-c49216870112"),
                TransferToId = Guid.Parse("e6786fad-435b-460f-b6dd-11dd32b3b6a6"),
                TransferStatus = TransferStatus.Processing,
                Items = new List<TransferItemDTO>()
                    {
                        new TransferItemDTO
                        {
                            ItemId = Guid.Parse("629b77d6-0256-4d35-a47a-53369042e645"),
                            Amount = 50
                        }
                    }
            });
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // Commit
        [Fact]
        public async Task CommitTransfer_CommitTransferSuccess()
        {
            var response = await _httpClient.PutAsync(_baseUrl + "/cefc9e60-7d37-41f5-b3c8-3144894f203e/commit", null);

            var result = await response.Content.ReadFromJsonAsync<TransferResponse>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
        }

        // Delete
        [Fact]
        public async Task DeleteTransfer_DeleteExistingTransfer_CheckIfTransfer()
        {
            string url = _baseUrl + "/4e889392-bd86-4305-a31f-db5d8d0ff17a";
            var deleteResponse = await _httpClient.DeleteAsync(url);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync(url);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetSingleTransfer_ReturnsNotFoundWithTransfer()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/758a3484-cedf-426f-b22c-f0aeed23bc37");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
