using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestServer.Endpoints
{
    [EndpointController("Animal")]
    [KnownTypes("Cow", typeof(Cow))]
    [KnownTypes("Dog", typeof(Dog))]
    [Post("[controller]/[type]")]
    public class AnimalEndpointHandler<TAnimal> : IJsonEndpointHandler<TAnimal, string> where TAnimal : IAnimal
    {
        public Task<string> Handle(TAnimal body, CancellationToken cancellationToken)
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
