using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace NetFabric.Benchmark
{
    [MemoryDiagnoser]
    public class ConstructorBenchmark
    {
        int[] collection;

        [Params(0, 10, 100)]
        public int Count {get; set;}

        [GlobalSetup]
        public void GlobalSetup()
        {
            collection = Enumerable.Range(0, Count).ToArray();
        }

        [Benchmark(Baseline = true)]
        public LinkedList<int> LinkedList() =>
            new LinkedList<int>(collection);

        [Benchmark]
        public DoubleLinkedList<int> DoubleLinkedList_Enumerable() =>
            new DoubleLinkedList<int>((IEnumerable<int>)collection);

        [Benchmark]
        public DoubleLinkedList<int> DoubleLinkedList_Array() =>
            new DoubleLinkedList<int>(collection);
    }
}
