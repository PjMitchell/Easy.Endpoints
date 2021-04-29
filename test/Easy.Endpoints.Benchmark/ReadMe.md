###Benchamarks
``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.928 (2004/?/20H1)
Intel Core i5-8250U CPU 1.60GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.102
  [Host]     : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT
  DefaultJob : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT


```
|       Method |      Mean |    Error |     StdDev |   Median | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------- |----------:|---------:|-----------:|---------:|-----:|-------:|------:|------:|----------:|
| GetEndPoint  |  62.73 μs |  0.960 μs |  1.551 μs |          |    1 | 2.5635 |     - |     - |   7.88 KB |
|      GetMvc  | 344.58 μs | 29.967 μs | 88.360 μs |          |    2 | 4.3945 |     - |     - |  14.35 KB |
| PostEndPoint |  102.5 μs |   0.91 μs |   0.76 μs | 102.3 μs |    1 | 3.0518 |     - |     - |   9.35 KB |
|      PostMvc |  434.9 μs |  45.22 μs | 133.34 μs | 473.5 μs |    2 | 4.8828 |     - |     - |  17.72 KB |