using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace NetFabric.Benchmark
{
    [MemoryDiagnoser]
    public class ConstructorBenchmark
    {
        [Params(0, 10, 100)]
        public int Count {get; set;}

        [Benchmark(Baseline = true)]
        public LinkedList<int> LinkedList() =>
            new LinkedList<int>(Enumerable.Range(0, Count));

        [Benchmark]
        public DoubleLinkedList<int> DoubleLinkedList() =>
            new DoubleLinkedList<int>(Enumerable.Range(0, Count));
    }
}
