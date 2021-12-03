using System.Threading.Tasks;
using Xunit;

namespace Easy.Endpoints.Analyzer.Test
{
    public class EasyEndpointAnalyzer_SyncForAsyncUnitTest
    {

        [Fact]
        public async Task WhenHandleAsync_Returns_Void_ReturnsWarning()
        {
            var test = @"
    using Easy.Endpoints;
    using System.Threading.Tasks;
    namespace ConsoleApplication1
    {
        public class GetTestEndpoint : IEndpoint
        {   
            public void HandleAsync()
            {
                
            }
        }
    }";

            var expected = EasyVerify<EasyEndpointAnalyzerAnalyzer>.Diagnostic(EasyEndpointWarnings.SyncForHandleAsync)
                .WithLocation(8, 25);
            await EasyVerify<EasyEndpointAnalyzerAnalyzer>.VerifyAnalyzerAsync(test, expected);
        }


        [Fact]
        public async Task WhenHandleAsync_Returns_Type_ReturnsWarning()
        {
            var test = @"
    using Easy.Endpoints;
    using System.Threading.Tasks;
    namespace ConsoleApplication1
    {
        public class GetTestEndpoint : IEndpoint
        {   
            public int HandleAsync()
            {
                return 1;
            }
        }
    }";

            var expected = EasyVerify<EasyEndpointAnalyzerAnalyzer>.Diagnostic(EasyEndpointWarnings.SyncForHandleAsync)
                .WithLocation(8, 24);
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
        public class GetTestEndpoint : IEndpoint
        {   
            public int HandleAsync()
            {
                return 1;
            }
        }
    }";

            var fix = @"
    using Easy.Endpoints;
    using System.Threading.Tasks;
    namespace ConsoleApplication1
    {
        public class GetTestEndpoint : IEndpoint
        {   
            public int Handle()
            {
                return 1;
            }
        }
    }";
            var expected = EasyVerify<EasyEndpointAnalyzerAnalyzer>.Diagnostic(EasyEndpointWarnings.SyncForHandleAsync)
                .WithLocation(8, 24);
            await EasyVerify<EasyEndpointAnalyzerAnalyzer, EasyEndpointAnalyzerCodeFixProvider>.VerifyCodeFixAsync(test, expected, fix);
        }
    }



}
