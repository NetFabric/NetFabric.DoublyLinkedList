using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace NetFabric.Benchmark
{
    [MarkdownExporterAttribute.GitHub]
    [MemoryDiagnoser]
    public class ConstructorBenchmark
    {
        IEnumerable<int>? enumerable;
        IReadOnlyList<int>? collection;

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
            new(enumerable!);

        [Benchmark]
        public LinkedList<int> LinkedList_List() =>
            new(collection!);

        [Benchmark]
        public DoublyLinkedList<int> DoublyLinkedList_Enumerable() =>
            new(enumerable!);

        [Benchmark]
        public DoublyLinkedList<int> DoublyLinkedList_List() =>
            new(collection!, reversed: false);

        [Benchmark]
        public DoublyLinkedList<int> DoublyLinkedList_List_Reversed() =>
            new(collection!, reversed: true);
    }
}
