using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class ObjectRequestHandler : IEndpointRequestHandler
    {
        private readonly ObjectEndpointMethodExecutor executor;
        protected readonly HttpContext httpContext;
        private readonly ParameterArrayFactory parameterFactory;
        private readonly EndpointOptions options;

        public ObjectRequestHandler(ObjectEndpointMethodExecutor executor, HttpContext httpContext, ParameterArrayFactory parameterFactory, EndpointOptions options)
        {
            this.executor = executor;
            this.httpContext = httpContext;
            this.parameterFactory = parameterFactory;
            this.options = options;
        }


        public async Task HandleRequest()
        {
            
            var parameters = await parameterFactory(httpContext);
            var endpointHandler = httpContext.RequestServices.GetRequiredService(executor.EndpointType);
            var result = await executor.Execute(endpointHandler, parameters);
            await HttpContextJsonHelper.WriteJsonResponse(httpContext,options.JsonSerializerOptions,result,executor.ObjectReturnType).ConfigureAwait(false);

        }
    }

}
