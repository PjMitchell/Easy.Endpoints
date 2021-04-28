using System;
using Xunit;

namespace Easy.Endpoints.Tests
{
    public class GuidIdParserTests
    {
        private readonly TestEndpointContextAccessor contextAccessor;
        private readonly IGuidIdRouteParser target;

        public GuidIdParserTests()
        {
            contextAccessor = new TestEndpointContextAccessor();
            target = new GuidIdRouteParser(contextAccessor);
        }


        [Fact]
        public void ParsesGuidId()
        {
            var id = "babc3b8f-87f1-42e1-a8f9-fc47e000e0af";
            contextAccessor.WithIdRouteParameters(id);
            var result = target.GetIdFromRoute();
            Assert.Equal(Guid.Parse(id), result);
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
