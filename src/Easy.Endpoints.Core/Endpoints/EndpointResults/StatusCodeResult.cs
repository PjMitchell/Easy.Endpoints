using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// A result that just updates the response status code
    /// </summary>
    public class StatusCodeResult : IEndpointResult
    {
        /// <summary>
        /// Creates a response with status code
        /// </summary>
        /// <param name="statusCode"></param>
        public StatusCodeResult(int statusCode)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Status code for response
        /// </summary>
        public int StatusCode { get; }

        /// <inheritdoc cref="IEndpointResult.ExecuteResultAsync"/>
        public ValueTask ExecuteResultAsync(HttpContext context, EndpointOptions options)
        {
            context.Response.StatusCode = StatusCode;
            return ValueTask.CompletedTask;
        }
    }
}
