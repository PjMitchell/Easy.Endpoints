using System;
using System.Globalization;
using System.Text.Json;

namespace Easy.Endpoints
{
    /// <summary>
    /// Option for Endpoints
    /// </summary>
    public class EndpointOptions
    {
        /// <summary>
        /// Creates new instance of EndpointOptions with default values
        /// </summary>
        public EndpointOptions()
        {
            JsonSerializer = new DefaultJsonSerializer();      
        }

        /// <summary>
        /// JSON Serializer for Endpoints
        /// </summary>
        public IJsonSerializer JsonSerializer { get; set; }

        /// <summary>
        /// Format provider for parsing Query and Route Parameters, defaults to Invariant Culture
        /// </summary>
        public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;
    }    
}
