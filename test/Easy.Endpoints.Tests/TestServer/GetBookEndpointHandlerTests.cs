using Easy.Endpoints.TestServer.Endpoints;
using Easy.Endpoints.TestServer.Endpoints.Books;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class GetBookEndpointHandlerTests
    {
        private readonly Microsoft.AspNetCore.TestHost.TestServer server;
        public GetBookEndpointHandlerTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer();
        }

        [Fact]
        public async Task GetBookEndpointHandlersCorrectly()
        {
            var httpResult = await server.CreateRequest("/Book").GetAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, httpResult.StatusCode);
            var observed = await httpResult.GetJsonBody<Book[]>();
            var expected = GetBookEndpointHandler.AllBooks().ToArray();
            Assert.Equal(expected.Length, observed.Length);
            for(var i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i].Id, observed[i].Id);
                Assert.Equal(expected[i].Name, observed[i].Name);
            }
        }
    }
}
