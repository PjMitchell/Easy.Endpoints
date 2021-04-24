﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Easy.Endpoints.Tests
{
    public class TestEndpointContextAccessor : IEndpointContextAccessor
    {
        private readonly Mock<HttpContext> context;
        public TestEndpointContextAccessor()
        {
            Response = new Mock<HttpResponse>();
            Request = new Mock<HttpRequest>();
            Request.Setup(s => s.RouteValues).Returns(new RouteValueDictionary());
            context = new Mock<HttpContext>();
            context.SetupGet(s => s.Request).Returns(Request.Object);
            context.SetupGet(s => s.Response).Returns(Response.Object);

        }
        public Mock<HttpResponse> Response { get; }
        public Mock<HttpRequest> Request { get; }

        public EndpointContext GetContext() => new EndpointContext(context.Object);
    }
}