using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints.People
{
    public class GetPeopleEndpoint : IEndpoint
    {
        private readonly IPeopleService peopleService;

        public GetPeopleEndpoint(IPeopleService peopleService)
        {
            this.peopleService = peopleService;
        }

        public Task<People[]> HandleAsync(string[] firstName, string[] surname, int? minAge, int? maxAge, CancellationToken cancellationToken)
        {
            return Task.FromResult(peopleService.AllPeople().Where(w =>
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

        
    }
}
