﻿using System.Threading.Tasks;

namespace Easy.Endpoints.TestServer.Endpoints
{
    [Get("TestOne")]
    public class GetTestResponseEndpointHandler : IJsonResponseEndpointHandler<TestResponsePayload>
    {
        public Task<TestResponsePayload> Handle() => Task.FromResult(TestResponsePayload.Default);
    }

    [Put("TestOne/{id:long:min(0)}")]
    public class PutTestResponseEndpoint : IJsonBodyEndpointHandler<TestResponsePayload>
    {
        private readonly IIntIdRouteParser idRouteParser;

        public PutTestResponseEndpoint(IIntIdRouteParser idRouteParser)
        {
            this.idRouteParser = idRouteParser;
        }

        public Task Handle(TestResponsePayload body)
        {
            var id = idRouteParser.GetIdFromRoute();
            return Task.CompletedTask;
        }
    }

    [Post("TestOne")]
    public class PostTestResponseEndpoint : IJsonBodyEndpointHandler<TestResponsePayload>
    {
        public Task Handle(TestResponsePayload body)
        {
            return Task.CompletedTask;
        }
    }

    [Post("TestTwo")]
    public class PostTestResponseAndBodyEndpoint : IJsonEndpointHandler<TestResponsePayload, TestResponsePayload>
    {
        public Task<TestResponsePayload> Handle(TestResponsePayload body)
        {
            return Task.FromResult(body);
        }
    }

    public class TestResponsePayload
    {
        public string PropertyOne { get; set; }
        public int PropertyTwo { get; set; }

        public static TestResponsePayload Default => new TestResponsePayload { PropertyOne = "TEaafjkoajgojao", PropertyTwo = 2352 };
    }
}
