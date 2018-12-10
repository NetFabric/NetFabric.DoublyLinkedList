using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace NetFabric.Benchmark
{
    [MemoryDiagnoser]
    public class EnumerationBenchmark
    {
        const int count = 10_000;
        LinkedList<int> linkedList;
        DoubleLinkedList<int> doubleLinkedList;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var data = Enumerable.Range(0, count);
            linkedList = new LinkedList<int>(data);
            doubleLinkedList = new DoubleLinkedList<int>(data);
        }
        
        [Benchmark(Baseline = true)]
        public int LinkedList_Forward_While() 
        {
            var count = 0;
            var current = linkedList.First;
            while (!(current is null))
            {
                count += current.Value;
                current = current.Next;
            }
            return count;
        }

        [Benchmark]
        public int LinkedList_Forward_ForEach() 
        {
            var count = 0;
            foreach (var value in linkedList)
                count += value;
            return count;
        }

        [Benchmark]
        public int LinkedList_Reverse_While() 
        {
            var count = 0;
            var current = linkedList.Last;
            while (!(current is null))
            {
                count += current.Value;
                current = current.Previous;
            }
            return count;
        }

        [Benchmark]
        public int LinkedList_Reverse_ForEach() 
        {
            var count = 0;
            foreach (var value in linkedList.Reverse())
                count += value;
            return count;
        }
        
        [Benchmark]
        public int DoubleLinkedList_Forward_While() 
        {
            var count = 0;
            var current = doubleLinkedList.First;
            while (!(current is null))
            {
                count += current.Value;
                current = current.Next;
            }
            return count;
        }

        [Benchmark]
        public int DoubleLinkedList_Forward_ForEach() 
        {
            var count = 0;
            foreach (var value in doubleLinkedList.EnumerateForward())
                count += value;
            return count;
        }
        
        [Benchmark]
        public int DoubleLinkedList_Reverse_While() 
        {
            var count = 0;
            var current = doubleLinkedList.Last;
            while (!(current is null))
            {
                count += current.Value;
                current = current.Previous;
            }
            return count;
        }

        [Benchmark]
        public int DoubleLinkedList_Reverse_ForEach() 
        {
            var count = 0;
            foreach (var value in doubleLinkedList.EnumerateReversed())
                count += value;
            return count;
        }
    }
}
