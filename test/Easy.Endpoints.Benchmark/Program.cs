using BenchmarkDotNet.Running;

namespace Easy.Endpoints.Benchmark
{
    public static class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<BookHttpJsonGet>();
            BenchmarkRunner.Run<BookHttpJsonPost>();

        }
    }
}
