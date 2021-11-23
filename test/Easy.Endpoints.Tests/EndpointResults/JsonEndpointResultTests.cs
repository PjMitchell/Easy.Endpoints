using Easy.Endpoints.TestService.Endpoints;
using Microsoft.AspNetCore.TestHost;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class JsonEndpointResultTests
    {
        private readonly TestServer server;
        public JsonEndpointResultTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(b => b.AddForEndpoint<JsonEndpoint>());
        }

        [Theory]
        [InlineData(200)]
        [InlineData(201)]
        [InlineData(404)]
        public async Task TestNoContentReturnsCorrectStatusCode(int id)
        {
            var httpResult = await server.CreateRequest($"/test/{id}").GetAsync();
            Assert.Equal(id, (int)httpResult.StatusCode);
            var observed = await httpResult.GetJsonBody<Book>();
            Assert.Equal(id, observed.Id);
            Assert.Equal(id.ToString(), observed.Name);

        }

        [Get("test/{id:int}")]
        private class JsonEndpoint : IEndpoint
        {
            public Task<IEndpointResult> HandleAsync(int id, CancellationToken cancellationToken)
            {
                return Task.FromResult(EndpointResult.Json(new Book { Id = id, Name = id.ToString() }, id));
            }
        }
    }
}
