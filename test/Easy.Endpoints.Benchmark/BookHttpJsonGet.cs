using BenchmarkDotNet.Attributes;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints.Benchmark
{
    [RankColumn, MemoryDiagnoser]
    public class BookHttpJsonGet
    {
        private HttpClient endPointServer;
        private HttpClient mvcServer;
        private HttpClient minimalServer;


        [GlobalSetup]
        public void Setup()
        {
            endPointServer = ServerFactory.CreateEndpointServer().CreateClient();
            mvcServer = ServerFactory.CreateMvcServer().CreateClient();
            minimalServer = ServerFactory.CreateMinimalApiServer().CreateClient();
        }

        [Benchmark]
        public async Task<string> GetEndPoint()
        {
            return await SendRequest(endPointServer, "/book").ConfigureAwait(false);
        }

        [Benchmark]
        public async Task<string> GetMvc()
        {
            return await SendRequest(mvcServer, "/book").ConfigureAwait(false);
        }

        [Benchmark]
        public async Task<string> GetMinimal()
        {
            return await SendRequest(minimalServer, "/book").ConfigureAwait(false);
        }


        private static async Task<string> SendRequest(HttpClient httpClient, string route = "/test1")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, route);
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidEndpointSetupException("Have not setup the test endpoints correctly");
            var output = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return output;
        }

    }


    [RankColumn, MemoryDiagnoser]
    public class TestPayload
    {


        [Benchmark]
        public string Run_StructWithFind()
        {
            return Run(StructWithFind);
        }

        [Benchmark]
        public string Run_StructWithError()
        {
            return Run(StructWithError);
        }

        [Benchmark]
        public string Run_StructWithNotFind()
        {
            return Run(StructWithNotFind);
        }

        [Benchmark]
        public string Run_ReadonlyStructWithFind()
        {
            return Run(ReadonlyStructWithFind);
        }

        [Benchmark]
        public string Run_ReadonlyStructWithError()
        {
            return Run(ReadonlyStructWithError);
        }

        [Benchmark]
        public string Run_ReadonlyStructWithNotFind()
        {
            return Run(ReadonlyStructWithNotFind);
        }


        private string Run(Func<TestReadonlyFlagStruct> get)
        {
            var result = get();
            if(result.Status == BindingState.None)
                return result.Result;
            if (result.Status.HasFlag(BindingState.Error))
                return "error";
            return "failed";
        }

        private string Run(Func<TestReadonlyStruct> get)
        {
            var result = get();
            if (result.IsFound)
                return result.Result;
            if (result.IsError)
                return "error";
            return "failed";
        }

        private TestReadonlyFlagStruct StructWithFind()
        {
            return new TestReadonlyFlagStruct { Status = BindingState.None, Result = "Hello" };
        }

        private TestReadonlyFlagStruct StructWithNotFind()
        {
            return new TestReadonlyFlagStruct { Status = BindingState.NotFound };
        }

        private TestReadonlyFlagStruct StructWithError()
        {
            return new TestReadonlyFlagStruct { Status = BindingState.Error };
        }

        private TestReadonlyStruct ReadonlyStructWithFind()
        {
            return new TestReadonlyStruct { IsFound = true, Result = "Hello" };
        }

        private TestReadonlyStruct ReadonlyStructWithNotFind()
        {
            return new TestReadonlyStruct { IsFound = false };
        }

        private TestReadonlyStruct ReadonlyStructWithError()
        {
            return new TestReadonlyStruct { IsError = true };
        }


        public class TestClass
        {
            public bool IsFound { get; set; }
            public bool IsError { get; set; }
            public string Result { get; set; }

        }

        public readonly struct TestReadonlyStruct
        {
            public TestReadonlyStruct(bool isFound, bool isError, string result)
            {
                IsFound = isFound;
                IsError = isError;
                Result = result;
            }
            public bool IsFound { get; init;  }
            public bool IsError { get; init; }
            public string Result { get; init; }

        }

        public readonly struct TestReadonlyFlagStruct
        {
            public TestReadonlyFlagStruct(string result, BindingState status)
            {
                Result = result;
                Status = status;
            }
            public string Result { get; init; }
            public BindingState Status { get; init; }

        }

        public struct TestStruct
        {
            public bool IsFound { get; set; }
            public bool IsError { get; set; }
            public string Result { get; set; }

        }

        [Flags]
        public enum BindingState : byte
        {
            None = 0,
            NotFound = 1,
            Error = 2,
        }
    }
}
