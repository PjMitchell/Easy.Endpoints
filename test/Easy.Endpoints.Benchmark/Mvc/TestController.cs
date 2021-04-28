using Easy.Endpoints.TestService.Endpoints;
using Easy.Endpoints.TestService.Endpoints.Books;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Easy.Endpoints.Benchmark.Mvc
{
    public class TestController : Controller
    {
        [HttpGet("Book")]
        public IActionResult Get()
        {
            return Ok(GetBookEndpointHandler.AllBooks().ToArray());
        }

        [HttpPost("Book")]
        public IActionResult Post([FromBody] Book book)
        {
            return Ok(new CommandResult { Successful = true, Message = book.Name });
        }
    }
}
