using System;
using BenchmarkDotNet.Running;

namespace NetFabric.Benchmark
{
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
