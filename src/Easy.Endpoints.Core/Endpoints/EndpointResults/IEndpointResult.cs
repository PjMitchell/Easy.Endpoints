using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// Defines an endpoint Result that modifies the endpoint context to produce the desired result
    /// </summary>
    public interface IEndpointResult
    {
        /// <summary>
        /// Modifies the endpoint context to produce the desired result
        /// </summary>
        /// <param name="context">Current endpoint context</param>
        /// <param name="options">EndpointOptions</param>
        /// <returns>A Task representing the operation</returns>
        ValueTask ExecuteResultAsync(HttpContext context, EndpointOptions options);
    }
}
