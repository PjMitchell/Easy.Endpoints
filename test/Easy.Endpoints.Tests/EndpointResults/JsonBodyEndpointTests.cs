using Easy.Endpoints.TestService.Endpoints;
using Microsoft.AspNetCore.TestHost;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class JsonBodyEndpointTests
    {
        private readonly TestServer server;
        public JsonBodyEndpointTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(b =>
            {
                b.WithEndpoint<JsonBodyEndpoint>();
                b.WithEndpoint<PutJsonBodyEndpoint>();

            });
        }

        [Fact]
        public async Task EndpointWithObjectBodyReturnsJson()
        {
            var book = new Book { Id = 1, Name = "testbook" };
            var httpResult = await server.CreateRequest("/test").AndJsonBody(book).PostAsync();
            Assert.Equal(200, (int)httpResult.StatusCode);
            var observed = await httpResult.GetJsonBody<Book>();
            Assert.Equal(book.Id, observed.Id);
            Assert.Equal(book.Name, observed.Name);

        }

        [Fact]
        public async Task EndpointWithObjectBodyAndRouteParamentReturnsJson()
        {
            var book = new Book { Id = 1, Name = "testbook" };
            var httpResult = await server.CreateRequest("/test/23").AndJsonBody(book).SendAsync("PUT");
            Assert.Equal(200, (int)httpResult.StatusCode);
            var observed = await httpResult.GetJsonBody<Book>();
            Assert.Equal(23, observed.Id);
            Assert.Equal(book.Name, observed.Name);

        }

        [Post("test")]
        private class JsonBodyEndpoint : IEndpoint
        {
            public Book Handle(Book book)
            {
                return book;
            }
        }

        [Put("test/{id:int}")]
        private class PutJsonBodyEndpoint : IEndpoint
        {
            public Book Handle(int id, Book book)
            {
                book.Id = id;
                return book;
            }
        }
    }
}
