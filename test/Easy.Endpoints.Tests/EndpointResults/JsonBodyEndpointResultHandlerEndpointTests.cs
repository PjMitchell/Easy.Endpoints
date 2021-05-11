using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class JsonBodyEndpointResultHandlerEndpointTests
    {
        private readonly TestEndpointContext testEndpointContext;
        private readonly Mock<IJsonBodyEndpointResultHandler<MockJsonBodyModel>> handler;
        private readonly JsonBodyEndpointResultHandlerEndpoint<IJsonBodyEndpointResultHandler<MockJsonBodyModel>, MockJsonBodyModel> target;
        public JsonBodyEndpointResultHandlerEndpointTests()
        {
            testEndpointContext = new TestEndpointContext();
            handler = new Mock<IJsonBodyEndpointResultHandler<MockJsonBodyModel>>();
            target = new JsonBodyEndpointResultHandlerEndpoint<IJsonBodyEndpointResultHandler<MockJsonBodyModel>, MockJsonBodyModel>(handler.Object);
        }

        [Fact]
        public async Task CallsHandlerAndExecutesResult()
        {
            var result = new Mock<IEndpointResult>();

            testEndpointContext.WithJsonBody(new MockJsonBodyModel());
            handler.Setup(s => s.HandleAsync(It.IsAny<MockJsonBodyModel>(), testEndpointContext.RequestAborted))
                .ReturnsAsync(result.Object);
            await target.HandleRequestAsync(testEndpointContext);
            result.Verify(v => v.ExecuteResultAsync(testEndpointContext), Times.Once);
        }

        [Fact]
        public async Task CallsHandlerWithJsonBody()
        {
            var result = new Mock<IEndpointResult>();
            var body = new MockJsonBodyModel { Id = 27, Value = "Test" };
            testEndpointContext.WithJsonBody(body);

            handler.Setup(s => s.HandleAsync(It.IsAny<MockJsonBodyModel>(), testEndpointContext.RequestAborted))
                .ReturnsAsync(result.Object);
            await target.HandleRequestAsync(testEndpointContext);
            handler.Verify(v => v.HandleAsync(It.Is<MockJsonBodyModel>(q => q.Id == body.Id && q.Value == body.Value), testEndpointContext.RequestAborted), Times.Once);
        }
    }
}
