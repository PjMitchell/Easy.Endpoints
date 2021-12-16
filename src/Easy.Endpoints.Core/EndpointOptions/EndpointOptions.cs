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
            JsonSerializerOptions = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            };
        }

        /// <summary>
        /// JSON Serializer Options for Endpoints
        /// </summary>
        public JsonSerializerOptions JsonSerializerOptions { get; set; }

        /// <summary>
        /// Format provider for parsing Query and Route Parameters, defaults to Invariant Culture
        /// </summary>
        public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;
    }
}
