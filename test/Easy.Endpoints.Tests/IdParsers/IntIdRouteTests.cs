using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class IntIdRouteTests
    {
        private readonly TestServer server;

        public IntIdRouteTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.AddForEndpoint<IdRouteEndpoint>());
        }


        [Fact]
        public async Task ParsesIntId()
        {
            var result = await Test("23");
            Assert.True(result.IsSuccessStatusCode);
            var body = await GetValue(result);
            Assert.Equal("23", body);
        }

        [Fact]
        public async Task Returns_404_IfWrongUrl()
        {
            var result = await Test("Wrong");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, result.StatusCode);
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

        [Get("Test/{id:int}")]
        public class IdRouteEndpoint : IEndpoint
        {
            public async Task<IEndpointResult> HandleAsync(int id, HttpResponse response)
            {
                response.StatusCode = 200;
                await response.WriteAsync(id.ToString());
                return EndpointResult.Completed();
            }
        }
    }
}
