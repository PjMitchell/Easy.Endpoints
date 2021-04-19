using Xunit;

namespace Easy.Endpoints.Tests
{
    public class IntIdParserTests
    {
        private readonly TestEndpointContextAccessor contextAccessor;
        private readonly IIntIdRouteParser target;

        public IntIdParserTests()
        {
            contextAccessor = new TestEndpointContextAccessor();
            target = new IntIdRouteParser(contextAccessor);
        }
        

        [Fact]
        public void ParsesIntId()
        {
            contextAccessor.WithIdRouteParameters("23");
            var result = target.GetIdFromRoute();
            Assert.Equal(23, result);
        }

        [Fact]
        public void Throws404StatusExceptionIfIdWrong()
        {
            contextAccessor.WithIdRouteParameters("Wrong!");
            var result = Assert.Throws<EndpointStatusCodeResponseException>(() => target.GetIdFromRoute());
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void Throws404StatusExceptionIfIdNotFound()
        {
            var result = Assert.Throws<EndpointStatusCodeResponseException>(() => target.GetIdFromRoute());
            Assert.Equal(404, result.StatusCode);
        }
    }    
}
