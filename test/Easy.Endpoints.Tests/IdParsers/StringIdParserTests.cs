using Xunit;

namespace Easy.Endpoints.Tests
{
    public class StringIdParserTests
    {
        private readonly TestEndpointContextAccessor contextAccessor;
        private readonly IStringIdRouteParser target;

        public StringIdParserTests()
        {
            contextAccessor = new TestEndpointContextAccessor();
            target = new StringIdRouteParser(contextAccessor);
        }


        [Fact]
        public void ParsesStringId()
        {
            var id = "TestId";
            contextAccessor.WithIdRouteParameters(id);
            var result = target.GetIdFromRoute();
            Assert.Equal(id, result);
        }

        [Fact]
        public void Throws404StatusExceptionIfIdNotFound()
        {
            var result = Assert.Throws<EndpointStatusCodeResponseException>(() => target.GetIdFromRoute());
            Assert.Equal(404, result.StatusCode);
        }
    }
}
