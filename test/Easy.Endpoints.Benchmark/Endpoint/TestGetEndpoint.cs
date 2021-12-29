using Easy.Endpoints.Benchmark.Mvc;
using Easy.Endpoints.TestService.Endpoints.People;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.Benchmark.Endpoint
{

    [Get("test1")]
    public class TestGetEndpoint : IEndpoint
    {
        public Task<TestResponsePayload> HandleAsync(CancellationToken cancellationToken) => Task.FromResult(TestResponsePayload.Default);
    }

    [Get("test2")]
    public class Test2GetEndpoint : IEndpoint
    {
        public Task<IEndpointResult> HandleAsync(CancellationToken cancellationToken) => Task.FromResult(EndpointResult.Ok(TestResponsePayload.Default));
    }

    [Get("People")]
    public class GetPeopleEndpoint : IEndpoint
    {
        public IEnumerable<People> Handle([FromQuery] PeopleQuery query) => GetPeople(query);

        public static IEnumerable<People> GetPeople(PeopleQuery query)
        {
            return AllPeople().Where(w =>
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
                    .ToArray();
        }

        public static IEnumerable<People> AllPeople() => PeopleService.AllPeople();
    }
}
