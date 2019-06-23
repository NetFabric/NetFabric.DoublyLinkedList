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
        DoublyLinkedList<int> doublyLinkedList;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var data = Enumerable.Range(0, count);
            linkedList = new LinkedList<int>(data);
            doublyLinkedList = new DoublyLinkedList<int>(data);
        }
        
        [Benchmark(Baseline = true)]
        public int LinkedList_Forward_While() 
        {
            var count = 0;
            var current = linkedList.First;
            while (current is object)
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
            while (current is object)
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
        public int DoublyLinkedList_Forward_While() 
        {
            var count = 0;
            var current = doublyLinkedList.First;
            while (current is object)
            {
                count += current.Value;
                current = current.Next;
            }
            return count;
        }

        [Benchmark]
        public int DoublyLinkedList_Forward_ForEach() 
        {
            var count = 0;
            foreach (var value in doublyLinkedList.EnumerateForward())
                count += value;
            return count;
        }
        
        [Benchmark]
        public int DoublyLinkedList_Reverse_While() 
        {
            var count = 0;
            var current = doublyLinkedList.Last;
            while (current is object)
            {
                count += current.Value;
                current = current.Previous;
            }
            return count;
        }

        [Benchmark]
        public int DoublyLinkedList_Reverse_ForEach() 
        {
            var count = 0;
            foreach (var value in doublyLinkedList.EnumerateReversed())
                count += value;
            return count;
        }
    }
}
