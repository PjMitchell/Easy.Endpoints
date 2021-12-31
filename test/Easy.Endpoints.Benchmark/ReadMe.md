### Benchamarks
``` ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i5-8250U CPU 1.60GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT


```
|      Method |     Mean |    Error |   StdDev | Rank |  Gen 0 | Allocated |
|------------ |---------:|---------:|---------:|-----:|-------:|----------:|
| GetEndPoint | 74.90 μs | 1.437 μs | 1.475 μs |    2 | 3.2959 |     10 KB |
|      GetMvc | 94.86 μs | 1.892 μs | 2.103 μs |    3 | 4.1504 |     13 KB |
|  GetMinimal | 73.29 μs | 1.209 μs | 1.009 μs |    1 | 3.2959 |     10 KB |

## Simple JSON Get Api
```
|      Method |     Mean |    Error |   StdDev | Rank |  Gen 0 | Allocated |
|------------ |---------:|---------:|---------:|-----:|-------:|----------:|
| GetEndPoint | 74.90 μs | 1.437 μs | 1.475 μs |    2 | 3.2959 |     10 KB |
|      GetMvc | 94.86 μs | 1.892 μs | 2.103 μs |    3 | 4.1504 |     13 KB |
|  GetMinimal | 73.29 μs | 1.209 μs | 1.009 μs |    1 | 3.2959 |     10 KB |


## Simple JSON Post Api

```
|       Method |      Mean |    Error |    StdDev | Rank |  Gen 0 | Allocated |
|------------- |----------:|---------:|----------:|-----:|-------:|----------:|
| PostEndPoint |  93.21 μs | 1.856 μs |  2.210 μs |    1 | 3.4180 |     12 KB |
|      PostMvc | 130.53 μs | 4.009 μs | 11.695 μs |    3 | 5.3711 |     17 KB |
|  PostMinimal |  96.64 μs | 1.907 μs |  3.852 μs |    2 | 3.4180 |     12 KB |


## Simple Get With Query object Api
```
|      Method |      Mean |    Error |   StdDev | Rank |  Gen 0 |  Gen 1 | Allocated |
|------------ |----------:|---------:|---------:|-----:|-------:|-------:|----------:|
| GetEndPoint |  81.27 μs | 1.377 μs | 1.288 μs |    1 | 3.6621 |      - |     12 KB |
|      GetMvc | 118.81 μs | 2.484 μs | 7.207 μs |    2 | 5.3711 | 0.4883 |     17 KB |