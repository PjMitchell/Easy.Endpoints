using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// Is build per request to an endpoint, once it is build from the HttpContext. It is used to handle the request.
    /// </summary>
    public interface IEndpointRequestHandler
    {

        /// <summary>
        /// Handles the Endpoint Request once 
        /// </summary>
        /// <returns>Returns a Task for the HandleReques Operation</returns>
        Task HandleRequest();
    }
    
}
