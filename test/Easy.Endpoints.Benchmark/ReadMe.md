### Benchamarks
``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.928 (2004/?/20H1)
Intel Core i5-8250U CPU 1.60GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.102
  [Host]     : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT
  DefaultJob : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT

## Simple JSON Get Api
```
|      Method |     Mean |    Error |   StdDev | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------ |---------:|---------:|---------:|-----:|-------:|------:|------:|----------:|
| GetEndPoint | 187.2 μs | 30.82 μs | 90.86 μs |    1 | 3.6621 |     - |     - |  11.61 KB |
|      GetMvc | 303.3 μs | 14.30 μs | 41.25 μs |    2 | 4.3945 |     - |     - |  14.37 KB |


## Simple JSON Post Api

```
|       Method |     Mean |    Error |   StdDev |   Median | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------- |---------:|---------:|---------:|---------:|-----:|-------:|------:|------:|----------:|
| PostEndPoint | 325.0 μs | 40.68 μs | 120.0 μs | 385.1 μs |    1 | 3.9063 |     - |     - |  13.04 KB |
|      PostMvc | 548.3 μs | 48.50 μs | 143.0 μs | 584.7 μs |    2 | 4.8828 |     - |     - |  17.73 KB |