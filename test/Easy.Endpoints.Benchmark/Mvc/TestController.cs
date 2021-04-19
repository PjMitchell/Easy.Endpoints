using Easy.Endpoints.TestServer.Endpoints;
using Easy.Endpoints.TestServer.Endpoints.Books;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Easy.Endpoints.Benchmark.Mvc
{
    public class TestController : Controller
    {
        [HttpGet("Book")]
        public Book[] Get()
        {
            return GetBookEndpointHandler.AllBooks().ToArray();
        }

        [HttpPost("Book")]
        public CommandResult Post([FromBody] Book book)
        {
            return new CommandResult { Successful = true, Message = book.Name };
        }
    }
}
