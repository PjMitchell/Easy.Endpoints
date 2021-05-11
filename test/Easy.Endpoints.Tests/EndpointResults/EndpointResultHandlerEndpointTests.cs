using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class EndpointResultHandlerEndpointTests
    {
        private readonly TestEndpointContext testEndpointContext;
        private readonly Mock<IEndpointResultHandler> handler;
        private readonly EndpointResultHandlerEndpoint<IEndpointResultHandler> target;
        public EndpointResultHandlerEndpointTests()
        {
            testEndpointContext = new TestEndpointContext();
            handler = new Mock<IEndpointResultHandler>();
            target = new EndpointResultHandlerEndpoint<IEndpointResultHandler>(handler.Object);
        }

        [Fact]
        public async Task CallsHandlerAndExecutesResult()
        {
            var result = new Mock<IEndpointResult>();
            handler.Setup(s => s.HandleAsync(testEndpointContext.RequestAborted))
                .ReturnsAsync(result.Object);
            await target.HandleRequestAsync(testEndpointContext);
            result.Verify(v => v.ExecuteResultAsync(testEndpointContext), Times.Once);
        }
    }
}
