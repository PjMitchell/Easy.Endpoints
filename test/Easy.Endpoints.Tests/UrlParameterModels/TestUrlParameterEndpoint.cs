using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Endpoints.Tests
{

    public abstract class TestUrlParameterEndpoint<T> : IEndpoint where T : UrlParameterModel, new()
    {
        public Task HandleRequest(EndpointContext endpointContext)
        {
            var model = new T();
            model.BindUrlParameters(endpointContext.Request);
            return endpointContext.Response.WriteAsJsonAsync(new TestUrlParameterEndpointResult<T>(model, model.Errors.ToList()));
        }
    }
}
