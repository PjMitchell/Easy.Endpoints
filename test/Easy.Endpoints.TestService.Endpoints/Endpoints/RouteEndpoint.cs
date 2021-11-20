using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints
{
    [Get("TestOne")]
    public class GetTestResponseEndpointHandler : IEndpoint
    {
        public Task<TestResponsePayload> HandleAsync(CancellationToken cancellationToken) => Task.FromResult(TestResponsePayload.Default);
    }

    [Put("TestOne/{id:int:min(0)}")]
    public class PutTestResponseEndpoint : IEndpoint
    {
        public Task HandleAsync(int id, TestResponsePayload body, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    [Post("TestOne")]
    public class PostTestResponseEndpoint : IEndpoint
    {
        public Task HandleAsync(TestResponsePayload body, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    [Post("TestTwo")]
    public class PostTestResponseAndBodyEndpoint : IEndpoint
    {
        public Task<TestResponsePayload> HandleAsync(TestResponsePayload body, CancellationToken cancellationToken)
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
