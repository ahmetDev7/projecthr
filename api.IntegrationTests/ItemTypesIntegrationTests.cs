using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace api.IntegrationTests
{
    public class ItemTypesIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "/api/ItemTypes";

        public ItemTypesIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions{
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetItemTypes_ItemTypesExist_ReturnsSuccesWithItemTypes()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();
                db.Database.Migrate();
                Seeding.IntializeTestDB(db);
            }

            var response = await _httpClient.GetAsync(_baseUrl);
            var result = await response.Content.ReadFromJsonAsync<List<ItemType>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().HaveCount(3);
        }
    }
}
