using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// A response with no content
    /// </summary>
    public class NoContentResult : IEndpointResult
    {
        /// <summary>
        /// Creates a response with no content
        /// </summary>
        /// <param name="statusCode"></param>
        public NoContentResult(int statusCode = 204)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Status code for response
        /// </summary>
        public int StatusCode { get; }

        /// <inheritdoc cref="IEndpointResult.ExecuteResultAsync"/>
        public Task ExecuteResultAsync(EndpointContext context)
        {
            context.Response.StatusCode = StatusCode;
            return Task.CompletedTask;
        }
    }
}
