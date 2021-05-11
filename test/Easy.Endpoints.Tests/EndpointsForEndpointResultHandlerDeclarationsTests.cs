using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class EndpointsForEndpointResultHandlerDeclarationsTests
    {
        private readonly IEndpointForHandlerDeclaration target;

        public EndpointsForEndpointResultHandlerDeclarationsTests()
        {
            target = new EndpointsForEndpointResultHandlerDeclarations();
        }

        [Fact]
        public void CanHandle_JsonBodyEndpointHandler()
        {
            var endpointType = target.GetEndpointForHandler(typeof(TestJsonBodyEndpointResultHandler).GetTypeInfo());

            Assert.NotNull(endpointType);
            Assert.Equal(typeof(JsonBodyEndpointResultHandlerEndpoint<TestJsonBodyEndpointResultHandler, TestBody>), endpointType);
        }

        [Fact]
        public void CanHandle_JsonBodyWithUrlParamenterEndpointHandler()
        {
            var endpointType = target.GetEndpointForHandler(typeof(TestJsonBodyWithUrlParametersEndpointResultHandler).GetTypeInfo());

            Assert.NotNull(endpointType);
            Assert.Equal(typeof(JsonBodyWithUrlParametersEndpointResultHandlerEndpoint<TestJsonBodyWithUrlParametersEndpointResultHandler, TestBody, TestUrlParameters>), endpointType);
        }

        [Fact]
        public void CanHandle_UrlParametersEndpointHandler()
        {
            var endpointType = target.GetEndpointForHandler(typeof(TestUrlParametersEndpointResultHandler).GetTypeInfo());

            Assert.NotNull(endpointType);
            Assert.Equal(typeof(UrlParametersEndpointResultHandlerEndpoint<TestUrlParametersEndpointResultHandler, TestUrlParameters>), endpointType);
        }

        [Fact]
        public void CanHandle_EndpointResultHandler()
        {
            var endpointType = target.GetEndpointForHandler(typeof(TestEndpointResultHandler).GetTypeInfo());

            Assert.NotNull(endpointType);
            Assert.Equal(typeof(EndpointResultHandlerEndpoint<TestEndpointResultHandler>), endpointType);
        }


        
        private class TestJsonBodyEndpointResultHandler : IJsonBodyEndpointResultHandler<TestBody>
        {
            Task<IEndpointResult> IJsonBodyEndpointResultHandler<TestBody>.HandleAsync(TestBody body, CancellationToken cancellationToken)
            {
                return Task.FromResult<IEndpointResult>(new NoContentResult());
            }
        }
        private class TestJsonBodyWithUrlParametersEndpointResultHandler : IJsonBodyWithUrlParametersEndpointResultHandler<TestBody, TestUrlParameters>
        {
            Task<IEndpointResult> IJsonBodyWithUrlParametersEndpointResultHandler<TestBody, TestUrlParameters>.HandleAsync(TestBody body, TestUrlParameters urlParameters, CancellationToken cancellationToken)
            {
                return Task.FromResult<IEndpointResult>(new NoContentResult());
            }
        }

        private class TestUrlParametersEndpointResultHandler : IUrlParametersEndpointResultHandler<TestUrlParameters>
        {
            public Task<IEndpointResult> HandleAsync(TestUrlParameters urlParameters, CancellationToken cancellationToken)
            {
                return Task.FromResult<IEndpointResult>(new NoContentResult());
            }
        }

        private class TestEndpointResultHandler : IEndpointResultHandler
        {
            public Task<IEndpointResult> HandleAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult<IEndpointResult>(new NoContentResult());
            }
        }

        private class TestBody { }

        private class TestUrlParameters : UrlParameterModel
        {
            public override void BindUrlParameters(HttpRequest request)
            {
            }
        }
    }
}
