using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Host;
using Xunit;
using RigoFunc.ApiCore.Internal;
using Love.Net.Core;

namespace RigoFunc.ApiCore.IntegrationTest {
    public class IOSError {
        public InvokeError Error { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ApiResultTest {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public ApiResultTest() {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task Api_Android_Test() {
            // Act
            _client.DefaultRequestHeaders.Add("Device", "android");
            var response = await _client.GetAsync($"/api/values/");
            response.EnsureSuccessStatusCode();

            var apiResult = await response.Content.ReadAsAsync<ApiResult<IEnumerable<string>>>();

            Assert.NotNull(apiResult);
            Assert.IsType<IEnumerable<string>>(apiResult.Data);
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

        [Theory]
        [InlineData(1)]
        public async Task Api_Android_Request_Test(int id) {
            // Act
            _client.DefaultRequestHeaders.Add("Device", "android");
            var response = await _client.GetAsync($"/api/values/{id}");
            response.EnsureSuccessStatusCode();

            var apiResult = await response.Content.ReadAsAsync<ApiResult<DateTime>>();

            Assert.NotNull(apiResult);
            Assert.IsType<DateTime>(apiResult.Data);
        }

        [Fact]
        public async Task Api_NULL_Return_Android_Request_Test() {
            _client.DefaultRequestHeaders.Add("Device", "android");
            var response = await _client.PostAsJsonAsync($"/api/values/null", "xUnit Test");
            response.EnsureSuccessStatusCode();

            var apiResult = await response.Content.ReadAsAsync<ApiResult>();

            Assert.NotNull(apiResult);
            Assert.Equal(404, apiResult.Code);
        }

        [Fact]
        public async Task Api_Void_Return_Android_Request_Test() {
            _client.DefaultRequestHeaders.Add("Device", "android");
            var response = await _client.PostAsJsonAsync($"/api/values/", "xUnit Test");
            response.EnsureSuccessStatusCode();

            var apiResult = await response.Content.ReadAsAsync<ApiResult<bool>>();

            Assert.NotNull(apiResult);
            Assert.Equal(0, apiResult.Code);
            Assert.True(apiResult.Data);
        }

        [Fact]
        public async Task Api_Task_Return_Android_Request_Test() {
            _client.DefaultRequestHeaders.Add("Device", "android");
            var response = await _client.PostAsJsonAsync($"/api/values/taskvoid", "xUnit Test");
            response.EnsureSuccessStatusCode();

            var apiResult = await response.Content.ReadAsAsync<ApiResult<bool>>();

            Assert.NotNull(apiResult);
            Assert.Equal(0, apiResult.Code);
            Assert.True(apiResult.Data);
        }

        [Fact]
        public async Task Api_Error_Android_Request_Test() {
            _client.DefaultRequestHeaders.Add("Device", "android");
            var response = await _client.PostAsJsonAsync($"/api/values/error", "xUnit Test");
            response.EnsureSuccessStatusCode();

            var apiResult = await response.Content.ReadAsAsync<ApiResult<InvokeError>>();

            Assert.NotNull(apiResult);
            Assert.Equal(400, apiResult.Code);

            Assert.Equal("Error", apiResult.Data.Code);
            Assert.Equal("xUnit Test", apiResult.Data.Message);
        }

        [Fact]
        public async Task Api_Error_IOS_Request_Test() {
            _client.DefaultRequestHeaders.Add("Device", "ios");
            var response = await _client.PostAsJsonAsync($"/api/values/error", "xUnit Test");

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

            var result = await response.Content.ReadAsAsync<IOSError>();

            Assert.NotNull(result);
        }
    }
}
