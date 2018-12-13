using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace NetFabric.Benchmark
{
    [MemoryDiagnoser]
    public class ConstructorBenchmark
    {
        IEnumerable<int> enumerable;
        IReadOnlyList<int> collection;

        [Params(0, 10, 100)]
        public int Count {get; set;}

        [GlobalSetup]
        public void GlobalSetup()
        {
            enumerable = Enumerable.Range(0, Count);
            collection = enumerable.ToList();
        }

        [Benchmark(Baseline = true)]
        public LinkedList<int> LinkedList_Enumerable() =>
            new LinkedList<int>(enumerable);

        [Benchmark]
        public LinkedList<int> LinkedList_List() =>
            new LinkedList<int>(collection);

        [Benchmark]
        public DoubleLinkedList<int> DoubleLinkedList_Enumerable() =>
            new DoubleLinkedList<int>(enumerable);

        [Benchmark]
        public DoubleLinkedList<int> DoubleLinkedList_List() =>
            new DoubleLinkedList<int>(collection, false);

        [Benchmark]
        public DoubleLinkedList<int> DoubleLinkedList_List_Reversed() =>
            new DoubleLinkedList<int>(collection, true);
    }
}
