using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TennisBooking.Merchandise.Api.Integration.Tests.Model;
using TennisBookings.Merchandise.Api;
using Xunit;

namespace TennisBooking.Merchandise.Api.Integration.Tests.Controllers
{
    public class CategoriesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        public CategoriesControllerTests(WebApplicationFactory<Startup> factory)
        {
            //_httpClient = factory.CreateDefaultClient(new Uri("http://localhost/api/categories"));
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/categories");
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsSuccessStatusCode()
        {
            var response = await _httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAll_ReturnsExpectedMediaType()
        {
            var response = await _httpClient.GetAsync("");

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task Get_ShouldReturnValidJson()
        {
            string[] categories = new string[]
            {
                "Accessories",
                "Bags",
                "Balls",
                "Clothing",
                "Rackets"
            };
            var responseStream = await _httpClient.GetStreamAsync("");
            var response = await JsonSerializer.DeserializeAsync<ExpectdCategoriesModel>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(categories.OrderBy(x => x), response.AllowedCategories.OrderBy(x => x));
        }
    }
}
