namespace Easy.Endpoints
{
    /// <summary>
    /// Extensions to the EasyEndpointBuilder to aid with adding endpoints
    /// </summary>
    public static class EasyEndpointBuilderExtensions
    {
        /// <summary>
        /// Add Endpoints For Implementation of IEndpoint;
        /// </summary>
        /// <typeparam name="TEndpoint">Type of IEndpoint to be added</typeparam>
        /// <param name="builder">The current instance of the manifest builder</param>
        /// <returns>Same instance of the EasyEndpointBuilder</returns>
        public static EasyEndpointBuilder WithEndpoint<TEndpoint>(this EasyEndpointBuilder builder) where TEndpoint : IEndpoint => builder.WithEndpoint(typeof(TEndpoint));

    }
}
