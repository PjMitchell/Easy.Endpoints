using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Tests
{

    public abstract class ParameterBindingTests<T> where T : struct
    {
        protected readonly TestServer server;
        protected abstract T expected1 { get; }
        protected abstract T expected2 { get; }
        protected abstract T expected3 { get; }

        protected ParameterBindingTests()
        {
            server = TestEndpointServerFactory.CreateEndpointServer(a => a.WithEndpoint<NumberEndpoint>().WithMalformedRequestHandler<ErrorHandler>());
        }

        [Fact]
        public async Task CanMapQueryParameters()
        {
            var result = await server.CreateRequest($"{expected3}/Test?single={expected2}&nullable={expected1}&multiple={expected1}&multiple={expected3}").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(expected1, observed.Nullable);
            Assert.Equal(expected2, observed.Single);
            Assert.Equal(expected3, observed.Route);
            Assert.Equal(new[] { expected1, expected3 }, observed.Multiple);

        }

        [Fact]
        public async Task MissingNullable_ReturnsNull()
        {
            var result = await server.CreateRequest($"{expected3}/Test?single={expected2}").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Null(observed.Nullable);
        }

        [Fact]
        public async Task MissingMultiple_ReturnsEmptyArray()
        {
            var result = await server.CreateRequest($"{expected3}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Empty(observed.Multiple);
        }

        [Fact]
        public async Task MissingSingle_Default()
        {
            var result = await server.CreateRequest($"{expected3}/Test").GetAsync();
            Assert.True(result.IsSuccessStatusCode);
            var observed = await result.GetJsonBody<UrlModel>();
            Assert.Equal(default(T), observed.Single);
        }

        [Fact]
        public async Task MultipleValues_ForNullable_Returns400()
        {
            var result = await server.CreateRequest($"{expected3}/Test?nullable={expected2}&nullable={expected1}").GetAsync();
            Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, result.StatusCode);
            var observed = await result.GetJsonBody<ErrorModel>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("nullable", error.Property);
            Assert.Equal("Multiple Values when expecting single", error.Error);

        }

        [Fact]
        public async Task MultipleValues_ForSingle_Returns400()
        {
            var result = await server.CreateRequest($"{expected3}/Test?single={expected2}&single={expected1}").GetAsync();
            Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, result.StatusCode);
            var observed = await result.GetJsonBody<ErrorModel>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("single", error.Property);
            Assert.Equal("Multiple Values when expecting single", error.Error);
        }

        [Fact]
        public async Task FailedToParses_ForNullable_Returns400()
        {
            var result = await server.CreateRequest($"{expected3}/Test?nullable=one").GetAsync();

            Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, result.StatusCode);
            var observed = await result.GetJsonBody<ErrorModel>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("nullable", error.Property);
            Assert.Equal("Could not parse one", error.Error);
        }

        [Fact]
        public async Task FailedToParses_ForSingle_Returns400()
        {
            var result = await server.CreateRequest($"{expected3}/Test?single=one").GetAsync();
            Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, result.StatusCode);
            var observed = await result.GetJsonBody<ErrorModel>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("single", error.Property);
            Assert.Equal("Could not parse one", error.Error);
        }

        [Fact]
        public async Task FailedToParses_ForMultiple_Returns400()
        {
            var result = await server.CreateRequest($"{expected3}/Test?multiple=one&multiple={expected2}").GetAsync();
            Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, result.StatusCode);
            var observed = await result.GetJsonBody<ErrorModel>();
            var error = Assert.Single(observed.Errors);
            Assert.Equal("multiple[0]", error.Property);
            Assert.Equal("Could not parse one", error.Error);
        }

        [Fact]
        public async Task FailedToParses_ForRoute_Returns404()
        {
            var result = await server.CreateRequest($"Nope/Test").GetAsync();
            Assert.Equal(System.Net.HttpStatusCode.NotFound, result.StatusCode);
        }

        [Get("{route}/Test")]
        public class NumberEndpoint : IEndpoint
        {
            public UrlModel Handle(T route, T? nullable, T[] multiple, T single = default(T))
            {
                return new UrlModel
                {
                    Route = route,
                    Single = single,
                    Nullable = nullable,
                    Multiple = multiple
                };
            }
        }

        public class UrlModel
        {
            public T[] Multiple { get; set; } = Array.Empty<T>();
            public T Single { get; set; }
            public T? Nullable { get; set; }
            public T Route { get; set; }

        }

        public class ErrorModel
        {
            public BindingError[] Errors { get; set; } = Array.Empty<BindingError>();
        }

        public class ErrorHandler : IMalformedRequestExceptionHandler
        {
            private readonly EndpointOptions options;

            public ErrorHandler(EndpointOptions options)
            {
                this.options = options;
            }
            public Task HandleMalformedRequest(MalformedRequestException ex, HttpContext httpContext)
            {
                httpContext.Response.StatusCode = 422;
                return options.JsonSerializer.SerializeToResponse(httpContext.Response, new ErrorModel { Errors = ex.BindingErrors }, httpContext.RequestAborted);
            }
        }
    }


    public class ByteParameterBindingTests : ParameterBindingTests<byte>
    {
        protected override byte expected1 => 2;
        protected override byte expected2 => 42;
        protected override byte expected3 => 50;
    }

    public class UShortParameterBindingTests : ParameterBindingTests<ushort>
    {
        protected override ushort expected1 => 2;
        protected override ushort expected2 => 42;
        protected override ushort expected3 => ushort.MaxValue;
    }

    public class ShortParameterBindingTests : ParameterBindingTests<short>
    {
        protected override short expected1 => 2;
        protected override short expected2 => -42;
        protected override short expected3 => 500;
    }

    public class UIntParameterBindingTests : ParameterBindingTests<uint>
    {
        protected override uint expected1 => 2;
        protected override uint expected2 => 42;
        protected override uint expected3 => uint.MaxValue;
    }

    public class IntParameterBindingTests : ParameterBindingTests<int>
    {
        protected override int expected1 => 2;
        protected override int expected2 => -42;
        protected override int expected3 => 500;
    }

    public class ULongParameterBindingTests : ParameterBindingTests<ulong>
    {
        protected override ulong expected1 => 2;
        protected override ulong expected2 => 42;
        protected override ulong expected3 => ulong.MaxValue;
    }

    public class LongParameterBindingTests : ParameterBindingTests<long>
    {
        protected override long expected1 => 2;
        protected override long expected2 => -42;
        protected override long expected3 => 500;
    }

    public class DoubleParameterBindingTests : ParameterBindingTests<double>
    {
        protected override double expected1 => 2.2;
        protected override double expected2 => -42.01;
        protected override double expected3 => 50.303;
    }

    public class FloatParameterBindingTests : ParameterBindingTests<float>
    {
        protected override float expected1 => 2.2f;
        protected override float expected2 => -42.01f;
        protected override float expected3 => 50.303f;
    }


    public class DecimalParameterBindingTests : ParameterBindingTests<decimal>
    {
        protected override decimal expected1 => 2.2m;
        protected override decimal expected2 => -42.01m;
        protected override decimal expected3 => 50.303m;
    }
}
