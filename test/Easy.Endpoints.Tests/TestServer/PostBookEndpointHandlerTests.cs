using Easy.Endpoints.TestService.Endpoints;
using Microsoft.AspNetCore.TestHost;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class PostBookEndpointHandlerTests
    {
        private readonly TestServer server;
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
