using Microsoft.AspNetCore.TestHost;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class NoContentEndpointTests
    {
        private readonly TestServer server;
        public NoContentEndpointTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(b => {
                b.WithEndpoint<NoContentEndpoint>();
                b.WithEndpoint<AsyncNoContentEndpoint>();                
            });
        }

        [Fact]
        public async Task EndpointWith201()
        {
            var httpResult = await server.CreateRequest("/test").GetAsync();
            Assert.Equal(201, (int)httpResult.StatusCode);
            var observed = await httpResult.Content.ReadAsStringAsync();
            Assert.Equal("", observed);

        }

        [Fact]
        public async Task AsyncEndpointWith201()
        {
            var httpResult = await server.CreateRequest("/testAsync").GetAsync();
            Assert.Equal(201, (int)httpResult.StatusCode);
            var observed = await httpResult.Content.ReadAsStringAsync();
            Assert.Equal("", observed);

        }

        [Get("test")]
        private class NoContentEndpoint : IEndpoint
        {
            public void Handle(CancellationToken cancellationToken) {}
        }

        [Get("testAsync")]
        private class AsyncNoContentEndpoint : IEndpoint
        {
            public Task Handle() => Task.CompletedTask;                
        }
        
    }
}
