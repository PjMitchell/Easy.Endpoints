using System;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal abstract class NoResultEndpointMethodExecutor : EndpointMethodExecutor
    {
        public NoResultEndpointMethodExecutor(Type endpointType) : base(endpointType)
        {
        }

        public abstract Task Execute(object instance, object?[] parameters);

        
    }
}
