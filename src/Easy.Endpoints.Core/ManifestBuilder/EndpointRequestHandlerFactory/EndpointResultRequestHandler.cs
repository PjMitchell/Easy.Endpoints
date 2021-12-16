﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class EndpointResultRequestHandler : IEndpointRequestHandler
    {
        private readonly ObjectEndpointMethodExecutor executor;
        protected readonly HttpContext httpContext;
        private readonly ParameterArrayFactory parameterFactory;

        public EndpointResultRequestHandler(ObjectEndpointMethodExecutor executor, HttpContext httpContext, ParameterArrayFactory parameterFactory)
        {
            this.executor = executor;
            this.httpContext = httpContext;
            this.parameterFactory = parameterFactory;
        }


        public async Task HandleRequest()
        {
            var options = httpContext.RequestServices.GetRequiredService<EndpointOptions>();
            var parameters = await parameterFactory(httpContext, options);
            var endpointHandler = httpContext.RequestServices.GetRequiredService(executor.EndpointType);
            var result = (IEndpointResult)await executor.Execute(endpointHandler, parameters);
            await result.ExecuteResultAsync(httpContext, options);

        }
    }

}
