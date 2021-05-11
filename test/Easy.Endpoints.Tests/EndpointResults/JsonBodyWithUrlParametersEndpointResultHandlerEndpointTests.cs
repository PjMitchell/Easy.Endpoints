using Moq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class JsonBodyWithUrlParametersEndpointResultHandlerEndpointTests
    {
        private readonly TestEndpointContext testEndpointContext;
        private readonly Mock<IJsonBodyWithUrlParametersEndpointResultHandler<MockJsonBodyModel, MockUrlParameterModel>> handler;
        private readonly JsonBodyWithUrlParametersEndpointResultHandlerEndpoint<IJsonBodyWithUrlParametersEndpointResultHandler<MockJsonBodyModel, MockUrlParameterModel>, MockJsonBodyModel, MockUrlParameterModel> target;
        public JsonBodyWithUrlParametersEndpointResultHandlerEndpointTests()
        {
            testEndpointContext = new TestEndpointContext();
            handler = new Mock<IJsonBodyWithUrlParametersEndpointResultHandler<MockJsonBodyModel, MockUrlParameterModel>>();
            target = new JsonBodyWithUrlParametersEndpointResultHandlerEndpoint<IJsonBodyWithUrlParametersEndpointResultHandler<MockJsonBodyModel, MockUrlParameterModel>, MockJsonBodyModel, MockUrlParameterModel>(handler.Object);
        }

        [Fact]
        public async Task CallsHandlerAndExecutesResult()
        {
            var result = new Mock<IEndpointResult>();

            testEndpointContext.WithJsonBody(new MockJsonBodyModel())
                .WithQueryParameter(MockUrlParameterModel.TestQueryParameter, "Test");
            handler.Setup(s => s.HandleAsync(It.IsAny<MockJsonBodyModel>(), It.IsAny<MockUrlParameterModel>(), testEndpointContext.RequestAborted))
                .ReturnsAsync(result.Object);
            await target.HandleRequestAsync(testEndpointContext);
            result.Verify(v => v.ExecuteResultAsync(testEndpointContext), Times.Once);
        }

        [Fact]
        public async Task CallsHandlerWithJsonBody()
        {
            var result = new Mock<IEndpointResult>();
            var body = new MockJsonBodyModel { Id = 27, Value = "Test" };
            testEndpointContext.WithJsonBody(body)
                .WithQueryParameter(MockUrlParameterModel.TestQueryParameter, "Test");

            handler.Setup(s => s.HandleAsync(It.IsAny<MockJsonBodyModel>(), It.IsAny<MockUrlParameterModel>(), testEndpointContext.RequestAborted))
                .ReturnsAsync(result.Object);
            await target.HandleRequestAsync(testEndpointContext);
            handler.Verify(v => v.HandleAsync(It.Is<MockJsonBodyModel>(q => q.Id == body.Id && q.Value == body.Value), It.IsAny<MockUrlParameterModel>(), testEndpointContext.RequestAborted), Times.Once);
        }

        [Fact]
        public async Task CallsHandlerWithMappedQueryParameter()
        {
            var result = new Mock<IEndpointResult>();

            testEndpointContext.WithJsonBody(new MockJsonBodyModel())
                .WithQueryParameter(MockUrlParameterModel.TestQueryParameter, "Test");
            handler.Setup(s => s.HandleAsync(It.IsAny<MockJsonBodyModel>(), It.IsAny<MockUrlParameterModel>(), testEndpointContext.RequestAborted))
                .ReturnsAsync(result.Object);
            await target.HandleRequestAsync(testEndpointContext);
            handler.Verify(v => v.HandleAsync(It.IsAny<MockJsonBodyModel>(), It.Is<MockUrlParameterModel>(q => q.TestValue == "Test"), testEndpointContext.RequestAborted), Times.Once);
        }

        [Fact]
        public async Task Returns400WithError_IfUrlParametersAreWrong()
        {
            var result = new Mock<IEndpointResult>();
            testEndpointContext.WithJsonBody(new MockJsonBodyModel());
            handler.Setup(s => s.HandleAsync(It.IsAny<MockJsonBodyModel>(), It.IsAny<MockUrlParameterModel>(), testEndpointContext.RequestAborted))
                .ReturnsAsync(result.Object);
            await target.HandleRequestAsync(testEndpointContext);
            handler.Verify(v => v.HandleAsync(It.IsAny<MockJsonBodyModel>(), It.IsAny<MockUrlParameterModel>(), testEndpointContext.RequestAborted), Times.Never);
            Assert.Equal(400, testEndpointContext.Response.StatusCode);
            var errors = JsonSerializer.Deserialize<UrlParameterModelError[]>(await testEndpointContext.ReadResponseBodyAsText(), testEndpointContext.JsonSerializerOptions);
            var error = Assert.Single(errors);
            Assert.Equal(MockUrlParameterModel.TestQueryParameter, error.ParameterName);
            Assert.Equal(MockUrlParameterModel.ErrorMessage, error.Error);

        }
    }
}
