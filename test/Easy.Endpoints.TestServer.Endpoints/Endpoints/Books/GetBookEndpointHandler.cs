using System;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestServer.Endpoints.Books
{
   
    public class GetBookEndpointHandler : IJsonResponseEndpointHandler<Book[]>
    {
        public Task<Book[]> Handle() => Task.FromResult(Array.Empty<Book>());
    }
}
