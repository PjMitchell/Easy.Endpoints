### Benchamarks
``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.928 (2004/?/20H1)
Intel Core i5-8500 CPU 3.00GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=5.0.202
  [Host]     : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  DefaultJob : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT


## Simple JSON Get Api
```
|      Method |     Mean |    Error |   StdDev | Rank |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------ |---------:|---------:|---------:|-----:|-------:|-------:|------:|----------:|
| GetEndPoint | 50.30 μs | 0.577 μs | 0.540 μs |    1 | 2.5024 | 0.0610 |     - |  11.61 KB |
|      GetMvc | 63.52 μs | 0.319 μs | 0.299 μs |    2 | 3.0518 |      - |     - |  14.35 KB |


## Simple JSON Post Api

```
|       Method |     Mean |    Error |   StdDev | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------- |---------:|---------:|---------:|-----:|-------:|------:|------:|----------:|
| PostEndPoint | 65.79 μs | 0.331 μs | 0.310 μs |    1 | 2.8076 |     - |     - |  13.07 KB |
|      PostMvc | 91.39 μs | 1.065 μs | 0.996 μs |    2 | 3.7842 |     - |     - |  17.73 KB |


## Simple Get With UrlParameters Api
```
|                   Method |     Mean |    Error |   StdDev | Rank |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------------- |---------:|---------:|---------:|-----:|-------:|-------:|------:|----------:|
|              GetEndPoint | 56.39 μs | 0.444 μs | 0.416 μs |    2 | 2.8076 | 0.0610 |     - |  12.92 KB |
| GetCodeGeneratedEndPoint | 54.77 μs | 0.598 μs | 0.560 μs |    1 | 2.7466 | 0.0610 |     - |  12.65 KB |
|                   GetMvc | 79.71 μs | 0.829 μs | 0.734 μs |    3 | 4.2725 | 0.1221 |     - |  20.02 KB |