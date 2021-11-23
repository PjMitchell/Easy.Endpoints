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
    public class GetPeopleByIdEndpoint : IEndpoint
    {
        private readonly IPeopleService peopleService;

        public GetPeopleByIdEndpoint(IPeopleService peopleService)
        {
            this.peopleService = peopleService;
        }

        public Task<IEndpointResult> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var person = peopleService.AllPeople().SingleOrDefault(p => p.Id == id);
            if (person is null)
                return Task.FromResult(EndpointResult.NotFound());
            return Task.FromResult(EndpointResult.Ok(person));
        }
    }
}
