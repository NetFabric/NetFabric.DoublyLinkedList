using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace NetFabric.Benchmark
{
    [MarkdownExporterAttribute.GitHub]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [MemoryDiagnoser]
    public class EnumerationBenchmark
    {
        LinkedList<int>? linkedList;
        DoublyLinkedList<int>? doublyLinkedList;
        
        [Params(100_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var data = Enumerable.Range(0, Count);
            linkedList = new LinkedList<int>(data);
            doublyLinkedList = new DoublyLinkedList<int>(data);
        }
        
        [BenchmarkCategory("Forward")]
        [Benchmark(Baseline = true)]
        public int LinkedList_Forward_While() 
        {
            var sum = 0;
            for (var current = linkedList!.First; current is not null; current = current.Next)
                sum += current.Value;
            return sum;
        }

        [BenchmarkCategory("Forward")]
        [Benchmark]
        public int LinkedList_Forward_ForEach() 
        {
            var sum = 0;
            foreach (var value in linkedList!)
                sum += value;
            return sum;
        }

        [BenchmarkCategory("Reverse")]
        [Benchmark(Baseline = true)]
        public int LinkedList_Reverse_While() 
        {
            var sum = 0;
            for (var current = linkedList!.Last; current is not null; current = current.Previous)
                sum += current.Value;
            return sum;
        }

        [BenchmarkCategory("Reverse")]
        [Benchmark]
        public int LinkedList_Reverse_ForEach() 
        {
            var sum = 0;
            foreach (var value in linkedList!.Reverse())
                sum += value;
            return sum;
        }
        
        [BenchmarkCategory("Forward")]
        [Benchmark]
        public int DoublyLinkedList_Forward_While() 
        {
            var sum = 0;
            for (var current = doublyLinkedList!.First; current is not null; current = current.Next)
                sum += current.Value;
            return sum;
        }

        [BenchmarkCategory("Forward")]
        [Benchmark]
        public int DoublyLinkedList_Forward_ForEach() 
        {
            var sum = 0;
            foreach (var value in doublyLinkedList!.Forward)
                sum += value;
            return sum;
        }
        
        [BenchmarkCategory("Reverse")]
        [Benchmark]
        public int DoublyLinkedList_Reverse_While() 
        {
            var sum = 0;
            for (var current = doublyLinkedList!.Last; current is not null; current = current.Previous)
                sum += current.Value;
            return sum;
        }

        [BenchmarkCategory("Reverse")]
        [Benchmark]
        public int DoublyLinkedList_Reverse_ForEach() 
        {
            var sum = 0;
            foreach (var value in doublyLinkedList!.Backward)
                sum += value;
            return sum;
        }
    }
}
