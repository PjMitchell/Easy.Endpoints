using Easy.Endpoints.TestService.Endpoints;
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
            server = TestEndpointServerFactory.CreateEndpointServer(b=> b.AddForEndpointHandler<NoContentEndpoint>());
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
        private class NoContentEndpoint : IEndpointResultHandler
        {
            private readonly IIntIdRouteParser intIdRouteParser;

            public NoContentEndpoint(IIntIdRouteParser intIdRouteParser)
            {
                this.intIdRouteParser = intIdRouteParser;
            }

            public Task<IEndpointResult> HandleAsync(CancellationToken cancellationToken)
            {
                var id = intIdRouteParser.GetIdFromRoute();
                return Task.FromResult<IEndpointResult>(new NoContentResult(id));
            }
        }
    }

    public class JsonEndpointResultTests
    {
        private readonly TestServer server;
        public JsonEndpointResultTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(b => b.AddForEndpointHandler<JsonEndpoint>());
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
        private class JsonEndpoint : IEndpointResultHandler
        {
            private readonly IIntIdRouteParser intIdRouteParser;

            public JsonEndpoint(IIntIdRouteParser intIdRouteParser)
            {
                this.intIdRouteParser = intIdRouteParser;
            }

            public Task<IEndpointResult> HandleAsync(CancellationToken cancellationToken)
            {
                var id = intIdRouteParser.GetIdFromRoute();
                return Task.FromResult<IEndpointResult>(new JsonContentResult<Book>(new Book { Id = id, Name = id.ToString() }, id));
            }
        }
    }
}
