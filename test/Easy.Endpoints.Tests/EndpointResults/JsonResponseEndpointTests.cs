using Easy.Endpoints.TestService.Endpoints;
using Microsoft.AspNetCore.TestHost;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class JsonResponseEndpointTests
    {
        private readonly TestServer server;
        public JsonResponseEndpointTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(b =>
            {
                b.AddForEndpoint<JsonResponseEndpoint>();
                b.AddForEndpoint<AsyncJsonResponseEndpoint>();
            });
        }

        [Fact]
        public async Task EndpointWithObjectBodyReturnsJson()
        {
            var httpResult = await server.CreateRequest("/test").GetAsync();
            Assert.Equal(200, (int)httpResult.StatusCode);
            var observed = await httpResult.GetJsonBody<Book[]>();
            var singleResult = Assert.Single(observed);
            Assert.Equal(1, singleResult.Id);
            Assert.Equal("Test", singleResult.Name);

        }

        [Fact]
        public async Task AsyncEndpointWithObjectBodyReturnsJson()
        {
            var httpResult = await server.CreateRequest("/testAsync").GetAsync();
            Assert.Equal(200, (int)httpResult.StatusCode);
            var observed = await httpResult.GetJsonBody<Book[]>();
            var singleResult = Assert.Single(observed);
            Assert.Equal(1, singleResult.Id);
            Assert.Equal("Test", singleResult.Name);

        }

        [Get("test")]
        private class JsonResponseEndpoint : IEndpoint
        {
            public Book[] Handle(CancellationToken cancellationToken)
            {
                return new[] { new Book { Id = 1, Name = "Test" } };
            }
        }

        [Get("testAsync")]
        private class AsyncJsonResponseEndpoint : IEndpoint
        {
            public Task<Book[]> HandleAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(new[] { new Book { Id = 1, Name = "Test" } });
            }
        }
    }
}
