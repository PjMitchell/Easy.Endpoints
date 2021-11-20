using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints
{
    [EndpointController("Animal")]
    [KnownTypes("Cow", typeof(Cow))]
    [KnownTypes("Dog", typeof(Dog))]
    [Post("[controller]/[type]")]
    public class AnimalEndpointHandler<TAnimal> : IEndpoint where TAnimal : IAnimal
    {
        public Task<string> HandleAsync(TAnimal body, CancellationToken cancellationToken)
        {
            return Task.FromResult(body.Says());
        }
    }

    public interface IAnimal
    {
        string Name { get; }
        string Says();
    }

    public class Cow : IAnimal
    {
        public string Name { get; set; }

        public string Says() => $"{Name} the cow says moo";
    }

    public class Dog : IAnimal
    {
        public string Name { get; set; }

        public string Says() => $"{Name} the dog says woof";
    }
}
