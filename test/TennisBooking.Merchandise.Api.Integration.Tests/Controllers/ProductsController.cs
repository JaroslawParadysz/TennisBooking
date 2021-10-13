using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TennisBooking.Merchandise.Api.Integration.Tests.Fakes;
using TennisBookings.Merchandise.Api;
using TennisBookings.Merchandise.Api.Data;
using Xunit;

namespace TennisBooking.Merchandise.Api.Integration.Tests.Controllers
{
    public class ProductsController : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ProductsController(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAll_ReturnsSuccessStatusCode()
        {
            var fakeProductRepository = new FakeProductDataRepository();

            HttpClient client = _factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(
                services => services.AddTransient<IProductDataRepository>(sys => fakeProductRepository)
            ))
            .CreateClient();

            HttpResponseMessage response = await client.GetAsync($"http://localhost/api/products/");
            response.EnsureSuccessStatusCode();

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }
    }
}
