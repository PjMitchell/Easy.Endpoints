using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints
{
    /// <summary>
    /// Declares Generic Parameter Type information for a generic endpoint
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public abstract class GenericParameterDiscoveryAttribute : Attribute, IGenericEndpointTypeInfoProvider
    {
        

        /// <inheritdoc cref="IGenericEndpointTypeInfoProvider.GetGenericEndpointTypeInfo"/>
        public IEnumerable<IGenericEndpointTypeInfo> GetGenericEndpointTypeInfo()
        {
            foreach (var typeInfo in GetTypes())
            {
                yield return new GenericEndpointTypeInfo(typeInfo, GetRouteVaulesForGenericParameter(typeInfo).ToArray());
            }
        }

        /// <summary>
        /// Gets All Generic Type Parameters to use
        /// </summary>
        /// <returns>All Generic Type Parameters to use</returns>
        protected abstract IEnumerable<Type[]> GetTypes();

        /// <summary>
        /// GetRouteVaulesForGenericParameter defaults to a single EndpointRouteValueMetadata with key: 'type' and value: type[o].Name
        /// </summary>
        /// <param name="types">Generic Type parameters</param>
        /// <returns>EndpointRouteValueMetadata values for generic type parameters</returns>
        protected virtual IEnumerable<EndpointRouteValueMetadata> GetRouteVaulesForGenericParameter(Type[] types)
        {
            yield return new EndpointRouteValueMetadata(EndpointRouteKeys.Type, types[0].Name);
        }
    }
}
