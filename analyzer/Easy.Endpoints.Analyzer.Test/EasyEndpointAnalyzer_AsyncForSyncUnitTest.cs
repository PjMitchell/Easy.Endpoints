using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Analyzer.Test
{
    public class EasyEndpointAnalyzer_AsyncForSyncUnitTest
    {

        [Fact]
        public async Task WhenHandle_Returns_Task_ReturnsWarning()
        {
            var test = @"
    using Easy.Endpoints;
    using System.Threading.Tasks;
    namespace ConsoleApplication1
    {
        public class GetTestEndpoint : IEndpoint
        {   
            public Task Handle()
            {
                return Task.CompletedTask;
            }
        }
    }";



            var expected = EasyVerify<EasyEndpointAnalyzerAnalyzer>.Diagnostic(EasyEndpointWarnings.AsyncForHandle)
                .WithLocation(8, 25);
            await EasyVerify<EasyEndpointAnalyzerAnalyzer>.VerifyAnalyzerAsync(test, expected);
        }


        [Fact]
        public async Task WhenHandle_Returns_Task_T_ReturnsWarning()
        {
            var test = @"
    using Easy.Endpoints;
    using System.Threading.Tasks;
    namespace ConsoleApplication1
    {
        public class GetTestEndpoint : IEndpoint
        {   
            public Task<int> Handle()
            {
                return Task.FromResult(1);
            }
        }
    }";



            var expected = EasyVerify<EasyEndpointAnalyzerAnalyzer>.Diagnostic(EasyEndpointWarnings.AsyncForHandle)
                .WithLocation(8, 30);
            await EasyVerify<EasyEndpointAnalyzerAnalyzer>.VerifyAnalyzerAsync(test, expected);
        }

        [Fact]
        public async Task WhenHandle_Returns_ValueTask_ReturnsWarning()
        {
            var test = @"
    using Easy.Endpoints;
    using System.Threading.Tasks;
    namespace ConsoleApplication1
    {
        public class GetTestEndpoint : IEndpoint
        {   
            public ValueTask Handle()
            {
                return ValueTask.CompletedTask;
            }
        }
    }";



            var expected = EasyVerify<EasyEndpointAnalyzerAnalyzer>.Diagnostic(EasyEndpointWarnings.AsyncForHandle)
                .WithLocation(8, 30);
            await EasyVerify<EasyEndpointAnalyzerAnalyzer>.VerifyAnalyzerAsync(test, expected);
        }


        [Fact]
        public async Task WhenHandle_Returns_ValueTask_T_ReturnsWarning()
        {
            var test = @"
    using Easy.Endpoints;
    using System.Threading.Tasks;
    namespace ConsoleApplication1
    {
        public class GetTestEndpoint : IEndpoint
        {   
            public ValueTask<int> Handle()
            {
                return ValueTask.FromResult(1);
            }
        }
    }";



            var expected = EasyVerify<EasyEndpointAnalyzerAnalyzer>.Diagnostic(EasyEndpointWarnings.AsyncForHandle)
                .WithLocation(8, 35);
            await EasyVerify<EasyEndpointAnalyzerAnalyzer>.VerifyAnalyzerAsync(test, expected);
        }

        [Fact]
        public async Task CanFix()
        {
            var test = @"
    using Easy.Endpoints;
    using System.Threading.Tasks;
    namespace ConsoleApplication1
    {
        internal class GetTestEndpoint : IEndpoint
        {   
            public ValueTask<int> Handle()
            {
                return ValueTask.FromResult(1);
            }
        }
    }";

            var fix = @"
    using Easy.Endpoints;
    using System.Threading.Tasks;
    namespace ConsoleApplication1
    {
        internal class GetTestEndpoint : IEndpoint
        {   
            public ValueTask<int> HandleAsync()
            {
                return ValueTask.FromResult(1);
            }
        }
    }";
            var expected = EasyVerify<EasyEndpointAnalyzerAnalyzer>.Diagnostic(EasyEndpointWarnings.AsyncForHandle)
                .WithLocation(8, 35);
            await EasyVerify<EasyEndpointAnalyzerAnalyzer, EasyEndpointAnalyzerCodeFixProvider>.VerifyCodeFixAsync(test, expected, fix);
        }
    }
}
