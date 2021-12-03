using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Meta data for Url Parameters
    /// </summary>
    public class UrlParameterMetaData
    {
        /// <summary>
        /// Creates new instance of UrlParameterMetaData
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="type">Paramter type</param>
        /// <param name="isOptional">Is Parameter optional</param>
        /// <param name="isRouteParameter">true if route paramter false if Query Parameter</param>
        public UrlParameterMetaData(string name, Type type, bool isOptional, bool isRouteParameter = false)
        {
            Name = name;
            Type = type;
            IsRouteParameter = isRouteParameter;
            IsOptional = isOptional;
        }

        /// <summary>
        /// Parameter Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Parameter Type
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Is Route Parameter, otherwise it is a query parameter
        /// </summary>
        public bool IsRouteParameter { get; }

        /// <summary>
        /// Is Parameter Optional
        /// </summary>
        public bool IsOptional { get; }
    }
}
