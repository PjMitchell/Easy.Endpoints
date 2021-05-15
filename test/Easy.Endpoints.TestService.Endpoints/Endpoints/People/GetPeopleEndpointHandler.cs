using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints.People
{
    public class GetPeopleEndpointHandler : IJsonResponseWithUrlParametersEndpointHandler<PeopleUrlParameters, People[]>
    {
        public Task<People[]> HandleAsync(PeopleUrlParameters urlParameters, CancellationToken cancellationToken)
        {
            return Task.FromResult(PeopleService.AllPeople().Where(w =>
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

        
    }
}
