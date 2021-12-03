using System;
using System.Collections;
using System.Collections.Generic;

namespace Easy.Endpoints
{
    /// <inheritdoc cref="IGenericEndpointTypeInfo"/>
    public class GenericEndpointTypeInfo : IGenericEndpointTypeInfo
    {
        private readonly ICollection<IEndpointRouteValueMetadataProvider> routeValues;

        /// <summary>
        /// Creates new instance of GenericEndpointTypeInfo
        /// </summary>
        /// <param name="typeParameters">Type parameters</param>
        /// <param name="routeValues">Endpoint route values</param>
        public GenericEndpointTypeInfo(Type[] typeParameters, params IEndpointRouteValueMetadataProvider[] routeValues)
        {
            TypeParameters = typeParameters;
            this.routeValues = routeValues;
        }

        /// <inheritdoc cref="IGenericEndpointTypeInfo.TypeParameters"/>
        public Type[] TypeParameters { get; }

        /// <summary>
        /// Gets Enumerator of IEndpointRouteValueMetadataProvider
        /// </summary>
        /// <returns>Enumerator of IEndpointRouteValueMetadataProvider</returns>
        public IEnumerator<IEndpointRouteValueMetadataProvider> GetEnumerator() => routeValues.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
