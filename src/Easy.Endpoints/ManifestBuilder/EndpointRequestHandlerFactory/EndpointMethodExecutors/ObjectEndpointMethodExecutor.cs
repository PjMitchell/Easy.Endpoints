using System;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal abstract class ObjectEndpointMethodExecutor : EndpointMethodExecutor
    {
        public abstract Type ObjectReturnType { get; }

        protected ObjectEndpointMethodExecutor(Type endpointType) : base(endpointType)
        {
        }

        public abstract Task<object> Execute(object instance, object?[] parameters);
    }
}
