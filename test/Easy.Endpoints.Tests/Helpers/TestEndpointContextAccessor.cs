using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.Tests
{
    public class TestEndpointContextAccessor : IEndpointContextAccessor
    {
        private readonly Mock<EndpointContext> context;
        public TestEndpointContextAccessor()
        {
            Response = new Mock<HttpResponse>();
            Request = new Mock<HttpRequest>();
            Request.Setup(s => s.RouteValues).Returns(new RouteValueDictionary());
            context = new Mock<EndpointContext>();
            context.SetupGet(s => s.Request).Returns(Request.Object);
            context.SetupGet(s => s.Response).Returns(Response.Object);
        }
        public Mock<HttpResponse> Response { get; }
        public Mock<HttpRequest> Request { get; }
        public EndpointContext GetContext() => context.Object;
    }

    public class TestEndpointContext: EndpointContext
    {
        private readonly Mock<HttpResponse> mockResponse;

        private int statusCode;
        private string contentType = string.Empty;
        private Stream resultStream;
        private readonly TestHttpRequest request;
        public TestEndpointContext()
        {
            JsonSerializerOptions = new JsonSerializerOptions();
            request = new TestHttpRequest(this);
            Features = new FeatureCollection();
            mockResponse = new Mock<HttpResponse>();
            Connection = new Mock<ConnectionInfo>().Object;
            WebSockets = Mock.Of<WebSocketManager>();
            User = new ClaimsPrincipal();
            Items = new Dictionary<object, object?>();
            RequestServices = Mock.Of<IServiceProvider>();
            Session = Mock.Of<ISession>();
            TraceIdentifier = "";
            resultStream = new MemoryStream();
            mockResponse.SetupGet(s => s.StatusCode).Returns(() => statusCode);
            mockResponse.SetupSet(s => s.StatusCode = It.IsAny<int>()).Callback((int v) => statusCode = v);
            mockResponse.SetupGet(s => s.ContentType).Returns(() => contentType);
            mockResponse.SetupSet(s => s.ContentType = It.IsAny<string>()).Callback((string v) => contentType = v);

            mockResponse.SetupGet(s => s.Body).Returns(() => resultStream);
            mockResponse.SetupSet(s => s.Body = It.IsAny<Stream>()).Callback((Stream v) => resultStream = v);
        }

        public override JsonSerializerOptions JsonSerializerOptions { get; }
        public override IFeatureCollection Features { get; }
        public override HttpRequest Request => request;
        public override HttpResponse Response => mockResponse.Object;
        public override ConnectionInfo Connection { get; }
        public override WebSocketManager WebSockets { get; }
        public override ClaimsPrincipal User { get; set; }
        public override IDictionary<object, object?> Items { get; set; }
        public override IServiceProvider RequestServices { get; set; }
        public override CancellationToken RequestAborted { get; set; }
        public override string TraceIdentifier { get; set; }
        public override ISession Session { get; set; }

        public override void Abort() { }

        public TestEndpointContext WithQueryParameter(string key, params string[] values)
        {
            request.Query = new QueryCollection(new Dictionary<string, StringValues> { [key] = new StringValues(values) });
            return this;
        }

        public TestEndpointContext WithJsonBody<T>(T obj)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(obj, JsonSerializerOptions);
            request.Body = new MemoryStream(bytes);
            request.ContentLength = bytes.Length;
            request.ContentType = "application/json";
            return this;
        }

        public async Task<string> ReadResponseBodyAsText()
        {
            resultStream.Position = 0;
            using var reader = new StreamReader(resultStream);
            var result = await reader.ReadToEndAsync();
            return result;

        }

        private class TestHttpRequest : HttpRequest
        {
            public TestHttpRequest(HttpContext context)
            {
                HttpContext = context;
                Query = new QueryCollection();
                Method = "GET";
                Headers = new HeaderDictionary();
                Body = new MemoryStream();
                Cookies = Mock.Of<IRequestCookieCollection>();
            }
            public override HttpContext HttpContext { get; }
            public override string Method { get; set; }
            public override string Scheme { get; set; } = string.Empty;
            public override bool IsHttps { get; set; }
            public override HostString Host { get; set; } 
            public override PathString PathBase { get; set; }
            public override PathString Path { get; set; }
            public override QueryString QueryString { get; set; }
            public override IQueryCollection Query { get; set; }
            public override string Protocol { get; set; } = string.Empty;
            public override IHeaderDictionary Headers { get; }
            public override IRequestCookieCollection Cookies { get; set; }
            public override long? ContentLength { get; set; }
            public override string? ContentType { get; set; }
            public override Stream Body { get; set; }
            public override bool HasFormContentType { get; }
            public override IFormCollection Form { get; set; } = FormCollection.Empty;

            public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default)
            {
                return Task.FromResult(Mock.Of<IFormCollection>());
            }
        }
    }
}
