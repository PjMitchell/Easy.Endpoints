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
    public class GetPeopleByIdEndpointHandler : IEndpointResultHandler
    {
        private readonly IIntIdRouteParser idRouteParser;

        public GetPeopleByIdEndpointHandler(IIntIdRouteParser idRouteParser)
        {
            this.idRouteParser = idRouteParser;
        }

        public Task<IEndpointResult> HandleAsync(CancellationToken cancellationToken)
        {
            var id = idRouteParser.GetIdFromRoute();
            var person = PeopleService.AllPeople().SingleOrDefault(p => p.Id == id);
            if (person is null)
                return Task.FromResult<IEndpointResult>(new NoContentResult(404));
            return Task.FromResult<IEndpointResult>(new JsonContentResult<People>(person));
        }
    }
}
