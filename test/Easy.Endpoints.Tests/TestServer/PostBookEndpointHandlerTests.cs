using Easy.Endpoints.TestServer.Endpoints;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class PostBookEndpointHandlerTests
    {
        private readonly Microsoft.AspNetCore.TestHost.TestServer server;
        public PostBookEndpointHandlerTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer();
        }

        [Fact]
        public async Task GetBookEndpointHandlersCorrectly()
        {
            var book = new Book { Id = 2, Name = "Test Book" };
            var httpResult = await server.CreateRequest("/Book").AndJsonBody(book).PostAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, httpResult.StatusCode);
            var observed = await httpResult.GetJsonBody<CommandResult>();
            Assert.True(observed.Successful);
            Assert.Equal($"Created {book.Name}", observed.Message);
        }
    }
}
