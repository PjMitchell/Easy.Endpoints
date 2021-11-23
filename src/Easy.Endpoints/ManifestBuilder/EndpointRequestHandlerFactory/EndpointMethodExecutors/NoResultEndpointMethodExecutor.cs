using System;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal abstract class NoResultEndpointMethodExecutor : EndpointMethodExecutor
    {
        protected NoResultEndpointMethodExecutor(Type endpointType) : base(endpointType)
        {
        }

        public abstract Task Execute(object instance, object?[] parameters);

        
    }
}
