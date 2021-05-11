using Moq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class UrlParametersEndpointResultHandlerEndpointTests
    {
        private readonly TestEndpointContext testEndpointContext;
        private readonly Mock<IUrlParametersEndpointResultHandler<MockUrlParameterModel>> handler;
        private readonly UrlParametersEndpointResultHandlerEndpoint<IUrlParametersEndpointResultHandler<MockUrlParameterModel>, MockUrlParameterModel> target;
        public UrlParametersEndpointResultHandlerEndpointTests()
        {
            testEndpointContext = new TestEndpointContext();
            handler = new Mock<IUrlParametersEndpointResultHandler<MockUrlParameterModel>>();
            target = new UrlParametersEndpointResultHandlerEndpoint<IUrlParametersEndpointResultHandler<MockUrlParameterModel>, MockUrlParameterModel>(handler.Object);
        }

        [Fact]
        public async Task CallsHandlerAndExecutesResult()
        {
            var result = new Mock<IEndpointResult>();

            testEndpointContext.WithQueryParameter(MockUrlParameterModel.TestQueryParameter, "Test");
            handler.Setup(s => s.HandleAsync(It.IsAny<MockUrlParameterModel>(),  testEndpointContext.RequestAborted))
                .ReturnsAsync(result.Object);
            await target.HandleRequestAsync(testEndpointContext);
            result.Verify(v => v.ExecuteResultAsync(testEndpointContext), Times.Once);
        }

        [Fact]
        public async Task CallsHandlerWithMappedQueryParameter()
        {
            var result = new Mock<IEndpointResult>();

            testEndpointContext.WithQueryParameter(MockUrlParameterModel.TestQueryParameter, "Test");
            handler.Setup(s => s.HandleAsync(It.IsAny<MockUrlParameterModel>(), testEndpointContext.RequestAborted))
                .ReturnsAsync(result.Object);
            await target.HandleRequestAsync(testEndpointContext);
            handler.Verify(v => v.HandleAsync(It.Is<MockUrlParameterModel>(q => q.TestValue == "Test"), testEndpointContext.RequestAborted), Times.Once);
        }

        [Fact]
        public async Task Returns400WithError_IfUrlParametersAreWrong()
        {
            var result = new Mock<IEndpointResult>();
            handler.Setup(s => s.HandleAsync(It.IsAny<MockUrlParameterModel>(), testEndpointContext.RequestAborted))
                .ReturnsAsync(result.Object);
            await target.HandleRequestAsync(testEndpointContext);
            handler.Verify(v => v.HandleAsync(It.IsAny<MockUrlParameterModel>(), testEndpointContext.RequestAborted), Times.Never);
            Assert.Equal(400, testEndpointContext.Response.StatusCode);
            var errors = JsonSerializer.Deserialize<UrlParameterModelError[]>(await testEndpointContext.ReadResponseBodyAsText(), testEndpointContext.JsonSerializerOptions);
            var error = Assert.Single(errors);
            Assert.Equal(MockUrlParameterModel.TestQueryParameter, error.ParameterName);
            Assert.Equal(MockUrlParameterModel.ErrorMessage, error.Error);

        }
    }
}
