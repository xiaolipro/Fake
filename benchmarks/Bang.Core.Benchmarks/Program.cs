// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Bang.Core.Benchmarks.Extensions.FastEnumerableExtensionsBenchmarks;
using Bang.Core.Benchmarks.Helpers.RandomHelperBenchmarks;

//BenchmarkRunner.Run<循环引用剪枝实验>();
BenchmarkRunner.Run<GUID种子对比默认种子>();
//Console.ReadKey();