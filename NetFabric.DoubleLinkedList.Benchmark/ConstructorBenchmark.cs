using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace NetFabric.Benchmark
{
    [MemoryDiagnoser]
    public class ConstructorBenchmark
    {
        List<int> collection;

        [Params(0, 10, 100)]
        public int Count {get; set;}

        [GlobalSetup]
        public void GlobalSetup()
        {
            collection = Enumerable.Range(0, Count).ToList();
        }

        [Benchmark(Baseline = true)]
        public LinkedList<int> LinkedList_Enumerable() =>
            new LinkedList<int>(collection);

        [Benchmark]
        public DoubleLinkedList<int> DoubleLinkedList_Enumerable() =>
            new DoubleLinkedList<int>((IEnumerable<int>)collection);

        [Benchmark]
        public DoubleLinkedList<int> DoubleLinkedList_List() =>
            new DoubleLinkedList<int>(collection, false);

        [Benchmark]
        public DoubleLinkedList<int> DoubleLinkedList_List_Reversed() =>
            new DoubleLinkedList<int>(collection, true);
    }
}
