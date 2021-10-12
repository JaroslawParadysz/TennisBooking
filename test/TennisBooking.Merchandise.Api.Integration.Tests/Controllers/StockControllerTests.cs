using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TennisBooking.Merchandise.Api.Integration.Tests.Fakes;
using TennisBooking.Merchandise.Api.Integration.Tests.Model;
using TennisBookings.Merchandise.Api;
using TennisBookings.Merchandise.Api.Data.Dto;
using TennisBookings.Merchandise.Api.External.Database;
using Xunit;

namespace TennisBooking.Merchandise.Api.Integration.Tests.Controllers
{
    public class StockControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly WebApplicationFactory<Startup> _factory;

        public StockControllerTests(WebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/stock/");
            _httpClient = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task GetStockTotal_ReturnsSuccessStatusCode()
        {
            var response = await _httpClient.GetAsync("total");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetStockTotal_ReturnsExpectedSjonContentString()
        {
            var response = await _httpClient.GetStringAsync("total");

            Assert.Equal("{\"stockItemTotal\":100}", response);
        }

        [Fact]
        public async Task GetStockTotal_ReturnExpectedJsonContentType()
        {
            var response = await _httpClient.GetAsync("total");
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task GetStockTotal_ReturnExpectedJson()
        {
            var responseStream = await _httpClient.GetStreamAsync("total");
            var model = await JsonSerializer.DeserializeAsync<StockTotalOutputModel>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(model);
            Assert.True(model.StockItemTotal > 0);
        }

        [Fact]
        public async Task GetStockTotal_ReturnsExpectedStockQuality()
        {
            var cloudDatabase = new FakeCloudDatabase(new[]
            {
                new ProductDto { StockCount = 200 },
                new ProductDto { StockCount = 500 },
                new ProductDto { StockCount = 300 }
            });

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(service => service.AddTransient<ICloudDatabase>(svc => cloudDatabase));
            }).CreateClient();

            var response = await client.GetAsync("http://localhost/api/stock/total");

            var model = await JsonSerializer.DeserializeAsync<StockTotalOutputModel>(await response.Content.ReadAsStreamAsync());

            Assert.Equal(1000, model.StockItemTotal);
        }
    }
}
