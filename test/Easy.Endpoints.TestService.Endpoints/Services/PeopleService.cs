using System.Collections.Generic;

namespace Easy.Endpoints.TestService.Endpoints.People
{
    public interface IPeopleService
    {
        IEnumerable<People> AllPeople();
    }

    public class PeopleService : IPeopleService
    {
        IEnumerable<People> IPeopleService.AllPeople() => AllPeople();

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
