using System;
using System.Net.Http;
using Xunit;
using FluentAssertions;

namespace projecthr
{
    public class TestLocation
    {
        private readonly HttpClient _client;

        public TestLocation()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/v1") };
            // _client.DefaultRequestHeaders.Add("Content-Type", "application/json");
        }


        [Fact]
        public async Task TestCreateLocation()
        {
            var newLocation = new
            {
                id = 115588,
                warehouse_id = 8,
                code = "MYTEST",
                name = "TEST LOCATION",
                created_at = "2024-10-15 10:21:32",
                updated_at = "2024-10-15 10:21:32"
            };
            var response = await _client.PostAsJsonAsync("/locations", newLocation);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

       
    }
}