using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// Endpoint result that returns a JSON Object
    /// </summary>
    /// <typeparam name="TResult">Result Type</typeparam>
    public class JsonContentResult<TResult>: IEndpointResult
    {

        /// <summary>
        /// Creates a new instance of JsonContentResult
        /// </summary>
        /// <param name="result">Object for JSON content</param>
        /// <param name="statusCode">Status code for response. defaults to 200</param>
        public JsonContentResult(TResult result, int statusCode = 200)
        {
            Result = result;
            StatusCode = statusCode;
        }

        /// <summary>
        /// Object for JSON content
        /// </summary>
        public TResult Result { get; }

        /// <summary>
        /// Status code for response
        /// </summary>
        public int StatusCode { get; }


        /// <inheritdoc cref="IEndpointResult.ExecuteResultAsync"/>
        public Task ExecuteResultAsync(EndpointContext context)
        {
            return HttpContextJsonHelper.WriteJsonResponse(context, Result, StatusCode);
        }
    }
}
