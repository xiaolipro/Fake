// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Fax.Core.Benchmarks.Collections;

BenchmarkRunner.Run<SortByDependenciesBenchmark>();
Console.ReadKey();