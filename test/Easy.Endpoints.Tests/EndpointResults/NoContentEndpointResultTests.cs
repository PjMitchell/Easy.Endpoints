using Microsoft.AspNetCore.TestHost;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class NoContentEndpointResultTests
    {
        private readonly TestServer server;
        public NoContentEndpointResultTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(b=> b.AddForEndpoint<NoContentEndpoint>());
        }

        [Theory]
        [InlineData(200)]
        [InlineData(201)]
        [InlineData(404)]
        public async Task TestNoContentReturnsCorrectStatusCode(int id)
        {
            var httpResult = await server.CreateRequest($"/test/{id}").GetAsync();
            Assert.Equal(id, (int)httpResult.StatusCode);
            var observed = await httpResult.Content.ReadAsStringAsync();
            Assert.Equal("", observed);
        }

        [Get("test/{id:int}")]
        private class NoContentEndpoint : IEndpoint
        {


            public Task<IEndpointResult> HandleAsync(int id, CancellationToken cancellationToken)
            {
                return Task.FromResult<IEndpointResult>(EndpointResult.StatusCode(id));
            }
        }
    }
}
