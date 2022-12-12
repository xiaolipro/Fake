// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Fax.Core.Benchmarks.Collections.ListExtensions;

BenchmarkRunner.Run<SortByDependenciesBenchmark>();
Console.ReadKey();