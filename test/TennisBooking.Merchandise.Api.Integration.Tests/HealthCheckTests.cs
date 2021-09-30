using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TennisBookings.Merchandise.Api;
using Xunit;

namespace TennisBooking.Merchandise.Api.Integration.Tests
{
    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        public HealthCheckTests(WebApplicationFactory<Startup> webApplicationFactory)
        {
            _client = webApplicationFactory.CreateDefaultClient();
        }

        [Fact]
        public async Task HealthCheck_ReturnsOk()
        {
            var resposne = await _client.GetAsync("/healthcheck");
            Assert.Equal(HttpStatusCode.OK, resposne.StatusCode);
        }
    }
}
