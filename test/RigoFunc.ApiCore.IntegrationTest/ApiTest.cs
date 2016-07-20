using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Host;
using Xunit;

namespace RigoFunc.ApiCore.IntegrationTest {
    public class ApiTest {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public ApiTest() {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Theory]
        [InlineData(1)]
        public async Task Api_Return_DateTime_Formatter_Test(int id) {
            // Act
            var response = await _client.GetAsync($"/api/values/{id}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.False(responseString.Contains("T"));
        }
    }
}
