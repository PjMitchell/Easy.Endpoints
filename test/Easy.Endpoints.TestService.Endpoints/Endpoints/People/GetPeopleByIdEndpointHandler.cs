using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints.People
{
    [ProducesResponseType(200,Type = typeof(People))]
    [ProducesResponseType(404, Type = typeof(void))]
    [Get("People/{id:int}")]
    public class GetPeopleByIdEndpointHandler : IEndpoint
    {
        public Task<IEndpointResult> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var person = PeopleService.AllPeople().SingleOrDefault(p => p.Id == id);
            if (person is null)
                return Task.FromResult(EndpointResult.NotFound());
            return Task.FromResult(EndpointResult.Ok(person));
        }
    }
}
