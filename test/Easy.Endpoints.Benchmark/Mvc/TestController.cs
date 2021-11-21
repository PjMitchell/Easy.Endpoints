using Easy.Endpoints.TestService.Endpoints;
using Easy.Endpoints.TestService.Endpoints.Books;
using Easy.Endpoints.TestService.Endpoints.People;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetPeople(string[] firstName, string[] surname, int? minAge, int? maxAge)
        {
            return Ok(AllPeople().Where(w =>
                {
                    if (firstName.Length != 0 && !firstName.Contains(w.FirstName))
                        return false;
                    if (surname.Length != 0 && !surname.Contains(w.Surname))
                        return false;

                    if (minAge.HasValue && w.Age < minAge.Value)
                        return false;

                    if (maxAge.HasValue && w.Age > maxAge.Value)
                        return false;

                    return true;
                })
                    .ToArray());
        }

        public static IEnumerable<People> AllPeople() => PeopleService.AllPeople();
    }
}
