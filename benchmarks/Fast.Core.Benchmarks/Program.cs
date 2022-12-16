// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Fast.Core.Benchmarks.Extensions.FastEnumerableExtensionsBenchmarks;
using Fast.Core.Benchmarks.Helpers.RandomHelperBenchmarks;

//BenchmarkRunner.Run<循环引用剪枝实验>();
BenchmarkRunner.Run<GUID种子对比默认种子>();
//Console.ReadKey();