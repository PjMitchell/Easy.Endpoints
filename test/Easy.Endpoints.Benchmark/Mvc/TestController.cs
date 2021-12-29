using Easy.Endpoints.TestService.Endpoints;
using Easy.Endpoints.TestService.Endpoints.Books;
using Easy.Endpoints.TestService.Endpoints.People;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints.Benchmark.Mvc
{
    public class TestController : Controller
    {
        [HttpGet("Book")]
        public IActionResult Get()
        {
            return Ok(GetBookEndpoint.AllBooks().ToArray());
        }

        [HttpPost("Book")]
        public IActionResult Post([FromBody] Book book)
        {
            return Ok(new CommandResult { Successful = true, Message = book.Name });
        }

        [HttpGet("People")]
        public IActionResult GetPeople([FromQuery] PeopleQuery query)
        {
            return Ok(AllPeople().Where(w =>
                {
                    if (query.FirstName.Length != 0 && !query.FirstName.Contains(w.FirstName))
                        return false;
                    if (query.Surname.Length != 0 && !query.Surname.Contains(w.Surname))
                        return false;

                    if (query.MinAge.HasValue && w.Age < query.MinAge.Value)
                        return false;

                    if (query.MaxAge.HasValue && w.Age > query.MaxAge.Value)
                        return false;

                    return true;
                })
                    .ToArray());
        }

        public static IEnumerable<People> AllPeople() => PeopleService.AllPeople();
    }

    public class PeopleQuery
    {
        public string[] FirstName { get; set; } = Array.Empty<string>();
        public string[] Surname { get; set; } = Array.Empty<string>();
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
    }
}
