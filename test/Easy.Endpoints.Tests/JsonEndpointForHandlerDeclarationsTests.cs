using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class JsonEndpointForHandlerDeclarationsTests
    {
        private readonly IEndpointForHandlerDeclaration target;

        public JsonEndpointForHandlerDeclarationsTests()
        {
            target = new JsonEndpointForHandlerDeclarations();
        }

        [Fact]
        public void CanHandle_JsonBodyEndpointHandler()
        {
            var endpointType = target.GetEndpointForHandler(typeof(TestJsonBodyEndpointHandler).GetTypeInfo());

            Assert.NotNull(endpointType);
            Assert.Equal(typeof(JsonBodyEndpoint<TestJsonBodyEndpointHandler, TestBody>), endpointType);
        }

        [Fact]
        public void CanHandle_JsonResponseEndpointHandler()
        {
            var endpointType = target.GetEndpointForHandler(typeof(TestJsonResponseEndpointHandler).GetTypeInfo());

            Assert.NotNull(endpointType);
            Assert.Equal(typeof(JsonResponseEndpoint<TestJsonResponseEndpointHandler, TestResponse>), endpointType);
        }

        [Fact]
        public void CanHandle_JsonResponseAndBodyEndpointHandler()
        {
            var endpointType = target.GetEndpointForHandler(typeof(TestJsonResponseAndBodyEndpointHandler).GetTypeInfo());

            Assert.NotNull(endpointType);
            Assert.Equal(typeof(JsonBodyAndResponseEndpoint<TestJsonResponseAndBodyEndpointHandler, TestBody, TestResponse>), endpointType);
        }

        [Fact]
        public void CanHandle_JsonBodyWithUrlParamenterEndpointHandler()
        {
            var endpointType = target.GetEndpointForHandler(typeof(TestJsonBodyWithUrlParamenterEndpointHandler).GetTypeInfo());

            Assert.NotNull(endpointType);
            Assert.Equal(typeof(JsonBodyWithUrlParametersEndpoint<TestJsonBodyWithUrlParamenterEndpointHandler, TestBody, TestUrlParameters>), endpointType);
        }

        [Fact]
        public void CanHandle_JsonResponseWithUrlParametersEndpointHandler()
        {
            var endpointType = target.GetEndpointForHandler(typeof(TestJsonResponseWithUrlParamenterEndpointHandler).GetTypeInfo());

            Assert.NotNull(endpointType);
            Assert.Equal(typeof(JsonResponseWithUrlParametersEndpoint<TestJsonResponseWithUrlParamenterEndpointHandler, TestUrlParameters, TestResponse>), endpointType);
        }

        [Fact]
        public void CanHandle_JsonBodyAndResponseWithUrlParametersEndpointHandler()
        {
            var endpointType = target.GetEndpointForHandler(typeof(TestJsonResponseAndBodyWithUrlParamenterEndpointHandler).GetTypeInfo());

            Assert.NotNull(endpointType);
            Assert.Equal(typeof(JsonBodyAndResponseWithUrlParametersEndpoint<TestJsonResponseAndBodyWithUrlParamenterEndpointHandler, TestBody, TestUrlParameters, TestResponse>), endpointType);
        }

        [Fact]
        public void CanHandle_UrlParameterHandler()
        {
            var endpointType = target.GetEndpointForHandler(typeof(TestUrlParameterHandler).GetTypeInfo());

            Assert.NotNull(endpointType);
            Assert.Equal(typeof(UrlParametersEndpoint<TestUrlParameterHandler,TestUrlParameters>), endpointType);
        }

        private class TestJsonBodyEndpointHandler : IJsonBodyEndpointHandler<TestBody>
        {
            public Task Handle(TestBody body, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        private class TestJsonResponseEndpointHandler : IJsonResponseEndpointHandler<TestResponse>
        {
            public Task<TestResponse> Handle(CancellationToken cancellationToken)
            {
                return Task.FromResult(new TestResponse());
            }
        }

        private class TestJsonResponseAndBodyEndpointHandler : IJsonBodyAndResponseEndpointHandler<TestBody, TestResponse>
        {
            public Task<TestResponse> Handle(TestBody body, CancellationToken cancellationToken)
            {
                return Task.FromResult(new TestResponse());
            }
        }

        private class TestJsonBodyWithUrlParamenterEndpointHandler : IJsonBodyWithUrlParametersEndpointHandler<TestBody, TestUrlParameters>
        {
            public Task Handle(TestBody body, TestUrlParameters urlParameters, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        private class TestJsonResponseWithUrlParamenterEndpointHandler : IJsonResponseWithUrlParametersEndpointHandler<TestUrlParameters, TestResponse>
        {
            public Task<TestResponse> Handle(TestUrlParameters urlParameters, CancellationToken cancellationToken)
            {
                return Task.FromResult(new TestResponse());
            }
        }

        private class TestJsonResponseAndBodyWithUrlParamenterEndpointHandler : IJsonBodyAndResponseWithUrlParametersEndpointHandler<TestBody, TestUrlParameters, TestResponse>
        {
            public Task<TestResponse> Handle(TestBody body, TestUrlParameters urlParameters, CancellationToken cancellationToken)
            {
                return Task.FromResult(new TestResponse());
            }
        }

        private class TestUrlParameterHandler : IUrlParametersEndpointHandler<TestUrlParameters>
        {
            public Task Handle(TestUrlParameters urlParameters, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        private class TestBody { }

        private class TestResponse { }

        private class TestUrlParameters : UrlParameterModel
        {
            public override void BindUrlParameters(HttpRequest request)
            {
            }
        }
    }
}
