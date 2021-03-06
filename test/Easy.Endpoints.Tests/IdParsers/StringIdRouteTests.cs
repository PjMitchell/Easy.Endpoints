using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class StringIdRouteTests
    {
        private readonly TestServer server;

        public StringIdRouteTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.WithEndpoint<IdRouteEndpoint>());
        }

        [Fact]
        public async Task ParsesStringId()
        {
            var result = await Test("Test");
            Assert.True(result.IsSuccessStatusCode);
            var body = await GetValue(result);
            Assert.Equal("Test", body);
        }

        private async Task<HttpResponseMessage> Test(string idPart)
        {
            var message = await server.CreateClient().GetAsync($"Test/{idPart}");
            return message;
        }

        private static Task<string> GetValue(HttpResponseMessage message)
        {
            return message.Content.ReadAsStringAsync();
        }

        [Get("Test/{id}")]
        public class IdRouteEndpoint : IEndpoint
        {
            public string HandleAsync(string id) => id;
        }
    }

}
