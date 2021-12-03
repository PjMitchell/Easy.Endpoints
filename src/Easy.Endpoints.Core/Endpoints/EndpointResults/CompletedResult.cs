using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// Result that does nothing further to the http Context
    /// </summary>
    public class CompletedResult : IEndpointResult
    {
        /// <inheritdoc cref="IEndpointResult.ExecuteResultAsync"/>
        public ValueTask ExecuteResultAsync(HttpContext context, EndpointOptions options)
        {
            return ValueTask.CompletedTask;
        }
    }
}
