using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class GuidIdRouteTests
    {
        private readonly TestServer server;

        public GuidIdRouteTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.WithEndpoint<IdRouteEndpoint>());
        }


        [Fact]
        public async Task ParsesGuidId()
        {
            var id = "babc3b8f-87f1-42e1-a8f9-fc47e000e0af";
            var result = await Test(id);
            Assert.True(result.IsSuccessStatusCode);
            var body = await GetValue(result);
            Assert.Equal(id, body);
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

        [Get("Test/{id:guid}")]
        public class IdRouteEndpoint : IEndpoint
        {           

            public Task<string> HandleAsync(Guid id)
            {
                return Task.FromResult(id.ToString());
            }
        }
    }
}
