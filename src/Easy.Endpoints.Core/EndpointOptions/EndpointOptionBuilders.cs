using System;
using System.Text.Json;

namespace Easy.Endpoints
{
    /// <summary>
    /// Used to construct the endpoint options
    /// </summary>
    [Obsolete("Not required any more")]
    public class EndpointOptionBuilders
    {
        private readonly EndpointOptions option;

        /// <summary>
        /// Constructs new EndpointOptionBuilders
        /// </summary>
        public EndpointOptionBuilders()
        {
            option = new EndpointOptions();
        }

        /// <summary>
        /// Adds Json Serializer Options, if not defined default settings will be used
        /// </summary>
        /// <param name="jsonSerializerOptions">jsonSerializerOptions to be used by endpoint</param>
        /// <returns>Updated instance of the option builder</returns>
        public EndpointOptionBuilders WithJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
        {
            option.JsonSerializer = new DefaultJsonSerializer(jsonSerializerOptions);
            return this;
        }

        /// <summary>
        /// Modifies default JsonSerializerOptions for Endpoints
        /// </summary>
        /// <param name="jsonSerializerOptionsModification">Action that modifies JsonSerializerOptions</param>
        /// <returns>Updated instance of the option builder</returns>
        public EndpointOptionBuilders WithJsonSerializer(Action<JsonSerializerOptions> jsonSerializerOptionsModification)
        {
            var jsonSerializerOptions = DefaultJsonSerializer.GetDefaultSettings();
            jsonSerializerOptionsModification(jsonSerializerOptions);
            option.JsonSerializer = new DefaultJsonSerializer(jsonSerializerOptions);
            return this;
        }

        /// <summary>
        /// Adds IFormatProvider, if not defined Invariant Culture will be used
        /// </summary>
        /// <param name="formatProvider">Format Provider to be used by endpoint</param>
        /// <returns>Updated instance of the option builder</returns>
        public EndpointOptionBuilders WithFormatProvider(IFormatProvider formatProvider)
        {
            option.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        /// Build Endpoint option
        /// </summary>
        /// <returns>Option Endpoint for builder</returns>
        public EndpointOptions BuildOption()
        {
            return option;
        }
    }
}
