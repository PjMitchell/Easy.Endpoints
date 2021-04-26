using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints
{
    /// <summary>
    /// Declares Generic Type information when there is only a single generic type parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public abstract class SingleGenericParameterDiscoveryAttribute : Attribute, IGenericEndpointTypeInfoProvider
    {
        
        /// <inheritdoc cref="IGenericEndpointTypeInfoProvider.GetGenericEndpointTypeInfo"/>
        public IEnumerable<IGenericEndpointTypeInfo> GetGenericEndpointTypeInfo()
        {
            foreach (var typeInfo in GetTypes())
            {
                yield return new GenericEndpointTypeInfo(new[] { typeInfo }, GetRouteVaulesForGenericParameter(typeInfo).ToArray());
            }
        }

        /// <summary>
        /// Gets All Generic Type Parameters to use
        /// </summary>
        /// <returns>All Generic Type Parameters to use</returns>
        protected abstract IEnumerable<Type> GetTypes();

        /// <summary>
        /// GetRouteVaulesForGenericParameter defaults to a single EndpointRouteValueMetadata with key: 'type' and value: type.Name
        /// </summary>
        /// <param name="type">Generic Type Parameter</param>
        /// <returns>EndpointRouteValueMetadata values for Type</returns>
        protected virtual IEnumerable<EndpointRouteValueMetadata> GetRouteVaulesForGenericParameter(Type type)
        {
            yield return new EndpointRouteValueMetadata(EndpointRouteKeys.Type, type.Name);
        }
    }
}
