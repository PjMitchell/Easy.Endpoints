using System.Linq;
using System.Reflection;

namespace Easy.Endpoints
{
    /// <summary>
    /// EndpointManifestBuilder for adding endpoints from an assembly
    /// </summary>
    public static class EndpointFromAssemblyExtensions
    {
        /// <summary>
        /// Adds all endpoints from a assembly to the manifest
        /// </summary>
        /// <param name="builder">The current instance of the manifest builder</param>
        /// <param name="assembly">Assembly to search for endpoints</param>
        /// <returns>Same instance of the manifest builder</returns>
        public static EndpointManifestBuilder AddFromAssembly(this EndpointManifestBuilder builder, Assembly assembly)
        {
            foreach (var endpoint in assembly.DefinedTypes.Where(IsRequestEndpoint))
            {
                builder.AddForEndpoint(endpoint);
            }

            foreach (var handler in assembly.DefinedTypes.Where(IsRequestHandler))
            {
                builder.AddForEndpointHandler(handler);
            }

            return builder;
        }

        private static bool IsRequestEndpoint(TypeInfo t) => t.IsAssignableTo(typeof(IEndpoint));
        private static bool IsRequestHandler(TypeInfo t) => t.IsAssignableTo(typeof(IEndpointHandler));
    }
}
