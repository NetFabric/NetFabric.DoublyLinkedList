using System;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Running;

namespace NetFabric.Benchmark
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static void Main(string[] args)
        {
            var switcher = new BenchmarkSwitcher(new[] {
                typeof(ConstructorBenchmark),
                typeof(EnumerationBenchmark),
            });
            switcher.Run(args);
        }
    }
}
