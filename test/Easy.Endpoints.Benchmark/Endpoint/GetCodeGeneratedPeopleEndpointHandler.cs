using Easy.Endpoints.TestService.Endpoints.People;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.Benchmark.Endpoint
{

    [Get("People")]
    public class GetCodeGeneratedPeopleEndpointHandler : IJsonResponseWithUrlParametersEndpointHandler<CodeGeneratedPeopleUrlParameters, People[]>
    {
        public Task<People[]> HandleAsync(CodeGeneratedPeopleUrlParameters urlParameters, CancellationToken cancellationToken)
        {
            return Task.FromResult(AllPeople().Where(w =>
            {
                if (urlParameters.FirstName.Length != 0 && !urlParameters.FirstName.Contains(w.FirstName))
                    return false;
                if (urlParameters.Surname.Length != 0 && !urlParameters.Surname.Contains(w.Surname))
                    return false;

                if (urlParameters.MinAge.HasValue && w.Age < urlParameters.MinAge.Value)
                    return false;

                if (urlParameters.MaxAge.HasValue && w.Age > urlParameters.MaxAge.Value)
                    return false;

                return true;
            })
                .ToArray());
        }

        public static IEnumerable<People> AllPeople()
        {
            return new[]
            {
                new People { Id = 1, FirstName = "Robert", Surname = "Walpole", Age = 45 },
                new People { Id = 2, FirstName = "Spencer", Surname = "Compton", Age = 73 },
                new People { Id = 3, FirstName = "Henry", Surname = "Pelham", Age = 49 },
                new People { Id = 4, FirstName = "Thomas", Surname = "Pelham-Holles", Age = 61 },
                new People { Id = 5, FirstName = "John", Surname = "Stuart", Age = 49 },
                new People { Id = 5, FirstName = "George", Surname = "Grenville", Age = 51 }
            };
        }
    }
}
