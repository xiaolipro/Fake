// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Fast.Core.Benchmarks.Collections;

BenchmarkRunner.Run<SortByDependenciesBenchmark>();
Console.ReadKey();