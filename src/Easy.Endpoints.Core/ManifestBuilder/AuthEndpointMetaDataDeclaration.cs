using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Easy.Endpoints
{
    /// <summary>
    /// Gets Auth Meta data for declared endpoint
    /// </summary>
    public class AuthEndpointMetaDataDeclaration : IEndpointMetaDataDeclaration
    {
        /// <summary>
        /// Gets Auth Meta data for declared endpoint
        /// </summary>
        /// <param name="declaredEndpoint">Declared endpoint</param>
        /// <returns>All Auth Meta data for declared endpoint</returns>
        public IEnumerable<object> GetMetaDataFromDeclaredEndpoint(TypeInfo declaredEndpoint)
        {
            return declaredEndpoint.GetCustomAttributes().Where(t => t is IAuthorizeData || t is IAllowAnonymous);            
        }
    }
}
