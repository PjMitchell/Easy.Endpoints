using System;
using System.Collections;
using System.Collections.Generic;

namespace Easy.Endpoints
{
    /// <summary>
    /// Declares known type parameters for a generic endpoint
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class KnownTypesAttribute: Attribute, IGenericEndpointTypeInfo
    {
        /// <summary>
        /// Creates new instance of KnownTypesAttribute for type and defined type name
        /// </summary>
        /// <param name="name">Name to be given to route value 'type'</param>
        /// <param name="typeParameters">Type Parameters for the endpoint</param>
        public KnownTypesAttribute(string name, params Type[] typeParameters)
        {
            TypeParameters = typeParameters;
            Name = name;
        }

        /// <summary>
        /// Creates new instance of KnownTypesAttribute for type and define type route parameter value as the first types name
        /// </summary>
        /// <param name="typeParameters">Type Parameters for the endpoint</param>
        public KnownTypesAttribute(params Type[] typeParameters)
        {
            TypeParameters = typeParameters;
            Name = typeParameters[0].Name;
        }

        /// <summary>
        /// Type name, used to provide 'type' route parameter
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Type parameter for generic endpoint
        /// </summary>
        public Type[] TypeParameters { get; }


        /// <summary>
        /// Gets all EndpointRouteValueMetaData for Generic type parameter
        /// </summary>
        /// <returns>All endpoint RouteValueMetaData for Generic type parameter</returns>
        public IEnumerator<IEndpointRouteValueMetadataProvider> GetEnumerator()
        {
            yield return new EndpointRouteValueMetadata(EndpointRouteKeys.Type, Name);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
