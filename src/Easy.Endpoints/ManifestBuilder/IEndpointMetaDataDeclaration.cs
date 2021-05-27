using System.Collections.Generic;
using System.Reflection;

namespace Easy.Endpoints
{
    /// <summary>
    /// Gets Meta data from Declared Endpoint TypeInfo
    /// </summary>
    public interface IEndpointMetaDataDeclaration
    {
        /// <summary>
        /// Gets Meta data from Declared Endpoint TypeInfo
        /// </summary>
        /// <param name="declaredEndpoint">Declared Endpoint TypeInfo</param>
        /// <returns>Endpoint Meta data for declared Endpoint</returns>
        IEnumerable<object> GetMetaDataFromDeclaredEndpoint(TypeInfo declaredEndpoint);
    }
}
