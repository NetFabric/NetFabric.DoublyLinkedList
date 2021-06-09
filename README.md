# NetFabric.DoublyLinkedList

![GitHub last commit (master)](https://img.shields.io/github/last-commit/NetFabric/NetFabric.DoublyLinkedList/master.svg?logo=github&logoColor=lightgray&style=popout-square)
[![Build](https://img.shields.io/github/workflow/status/NetFabric/NetFabric.DoublyLinkedList/.NET?style=flat-square&logo=github)](https://github.com/NetFabric/NetFabric.DoublyLinkedList/actions/workflows/dotnet.yml)
[![Coverage](https://img.shields.io/coveralls/github/NetFabric/NetFabric.DoublyLinkedList/master?style=flat-square&logo=coveralls)](https://coveralls.io/github/NetFabric/NetFabric.DoublyLinkedList)
[![NuGet Version](https://img.shields.io/nuget/v/NetFabric.DoublyLinkedList.svg?style=popout-square&logoColor=lightgray&logo=nuget)](https://www.nuget.org/packages/NetFabric.DoublyLinkedList/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/NetFabric.DoublyLinkedList.svg?style=popout-square&logoColor=lightgray&logo=nuget)](https://www.nuget.org/packages/NetFabric.DoublyLinkedList/)

An alternative to the [`System.Collections.Generic.LinkedList<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1) with reverse operation and enumeration without allocation.

The public API is very similar but the internals are very different with a more efficient implementation. 

New overrides and methods were added to minimizing the memory allocations, number of loops, conditions and assignments required in multiple scenarios.

## Benchmarks

Performance comparison between [`System.Collections.Generic.LinkedList<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1) and `DoublyLinkedList<T>`. 

The benchmarks project is part of the repository. You can check the code and run it on your machine.

### Constructor performance

|                         Method | Count |         Mean |      Error |     StdDev |       Median | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------------- |------ |-------------:|-----------:|-----------:|-------------:|------:|--------:|-------:|------:|------:|----------:|
|          **LinkedList_Enumerable** |     **0** |    **12.082 ns** |  **0.3174 ns** |  **0.4942 ns** |    **12.091 ns** |  **1.00** |    **0.00** | **0.0191** |     **-** |     **-** |      **40 B** |
|                LinkedList_List |     0 |    21.542 ns |  0.9611 ns |  2.7884 ns |    19.863 ns |  2.07 |    0.21 | 0.0382 |     - |     - |      80 B |
|    DoublyLinkedList_Enumerable |     0 |    10.749 ns |  0.0741 ns |  0.0657 ns |    10.761 ns |  0.89 |    0.04 | 0.0191 |     - |     - |      40 B |
|          DoublyLinkedList_List |     0 |     7.533 ns |  0.1320 ns |  0.1170 ns |     7.511 ns |  0.62 |    0.03 | 0.0191 |     - |     - |      40 B |
| DoublyLinkedList_List_Reversed |     0 |     7.391 ns |  0.1275 ns |  0.1192 ns |     7.386 ns |  0.61 |    0.03 | 0.0191 |     - |     - |      40 B |
|                                |       |              |            |            |              |       |         |        |       |       |           |
|          **LinkedList_Enumerable** |    **10** |   **205.166 ns** |  **1.2539 ns** |  **1.1116 ns** |   **205.147 ns** |  **1.00** |    **0.00** | **0.2677** |     **-** |     **-** |     **560 B** |
|                LinkedList_List |    10 |   241.563 ns |  1.7717 ns |  2.3651 ns |   241.213 ns |  1.18 |    0.01 | 0.2675 |     - |     - |     560 B |
|    DoublyLinkedList_Enumerable |    10 |   163.873 ns |  1.8013 ns |  1.5042 ns |   163.424 ns |  0.80 |    0.01 | 0.2677 |     - |     - |     560 B |
|          DoublyLinkedList_List |    10 |   142.060 ns |  1.3490 ns |  1.1265 ns |   142.538 ns |  0.69 |    0.01 | 0.2487 |     - |     - |     520 B |
| DoublyLinkedList_List_Reversed |    10 |   139.378 ns |  1.0579 ns |  0.8834 ns |   139.184 ns |  0.68 |    0.00 | 0.2487 |     - |     - |     520 B |
|                                |       |              |            |            |              |       |         |        |       |       |           |
|          **LinkedList_Enumerable** |   **100** | **1,804.348 ns** | **16.3064 ns** | **15.2530 ns** | **1,808.247 ns** |  **1.00** |    **0.00** | **2.3327** |     **-** |     **-** |   **4,880 B** |
|                LinkedList_List |   100 | 1,894.060 ns | 12.0334 ns | 11.2561 ns | 1,895.876 ns |  1.05 |    0.01 | 2.3327 |     - |     - |   4,880 B |
|    DoublyLinkedList_Enumerable |   100 | 1,355.031 ns | 16.9081 ns | 14.1191 ns | 1,352.691 ns |  0.75 |    0.01 | 2.3327 |     - |     - |   4,880 B |
|          DoublyLinkedList_List |   100 | 1,216.764 ns | 11.3984 ns | 10.6620 ns | 1,218.606 ns |  0.67 |    0.01 | 2.3136 |     - |     - |   4,840 B |
| DoublyLinkedList_List_Reversed |   100 | 1,233.906 ns | 11.7235 ns | 10.9662 ns | 1,238.096 ns |  0.68 |    0.01 | 2.3136 |     - |     - |   4,840 B |

### Enumeration performance

|                           Method | Categories |  Count |     Mean |   Error |  StdDev | Ratio | RatioSD |   Gen 0 |   Gen 1 |   Gen 2 | Allocated |
|--------------------------------- |----------- |------- |---------:|--------:|--------:|------:|--------:|--------:|--------:|--------:|----------:|
|         LinkedList_Forward_While |    Forward | 100000 | 320.0 μs | 3.77 μs | 3.53 μs |  1.00 |    0.00 |       - |       - |       - |         - |
|       LinkedList_Forward_ForEach |    Forward | 100000 | 496.1 μs | 5.55 μs | 5.19 μs |  1.55 |    0.02 |       - |       - |       - |         - |
|   DoublyLinkedList_Forward_While |    Forward | 100000 | 306.3 μs | 3.37 μs | 3.15 μs |  0.96 |    0.01 |       - |       - |       - |         - |
| DoublyLinkedList_Forward_ForEach |    Forward | 100000 | 470.0 μs | 4.44 μs | 3.70 μs |  1.47 |    0.01 |       - |       - |       - |         - |
|                                  |            |        |          |         |         |       |         |         |         |         |           |
|         LinkedList_Reverse_While |    Reverse | 100000 | 353.5 μs | 6.03 μs | 5.03 μs |  1.00 |    0.00 |       - |       - |       - |         - |
|       LinkedList_Reverse_ForEach |    Reverse | 100000 | 934.6 μs | 7.46 μs | 6.23 μs |  2.64 |    0.04 | 41.0156 | 41.0156 | 41.0156 | 400,393 B |
|   DoublyLinkedList_Reverse_While |    Reverse | 100000 | 356.4 μs | 2.89 μs | 2.70 μs |  1.01 |    0.02 |       - |       - |       - |         - |
| DoublyLinkedList_Reverse_ForEach |    Reverse | 100000 | 499.3 μs | 6.13 μs | 5.44 μs |  1.42 |    0.03 |       - |       - |       - |         - |


## Forward and reverse enumeration

`DoublyLinkedList<T>` does not directly implement `IEnumerable<T>`. Call the properties `Forward` or `Backward` to get an enumerator that goes in the direction you require.

```csharp
var list = new DoublyLinkedList<int>(new[] {1, 2, 3, 4});
foreach (var item in list.Backward)
	Console.Write(item);
```
outputs
```
4321
```

These methods allow the use of LINQ on either direction:

```csharp
var list = new DoublyLinkedList<int>(new[] {1, 2, 3, 4});
Console.Write(list.Backward.First());
```
outputs
```
4
```

Although these enumerators are optimized for performance, they perform a bit more method calls and conditions than simply using a `while` loop to go throught the `Node` references. The performance penalty can be seen on the benchmarks above. 

```csharp
var current = doublyLinkedList.Last;
while (current is object)
{
    Console.Write(current.Value);
    current = current.Previous;
}
```

The enumerators also supports random access by using an indexer. It allows indexing relative to the head or the tail of the list.

```csharp
var head = doublyLinkedList.Forward[0];
var tail = doublyLinkedList.Backward[0];
```

Please note that this `DoublyLinkedList` is not optimized for random access. It will use the shortest path to the item but it will have to step through the list to get there.

## `AddFirst()` and `AddLast()`

These methods now support the addition of collections of items. These can come from an `IEnumerable<T>`, `IReadOnlyList<T>` or another `DoublyLinkedList<T>`. 

When the parameter is `IReadOnlyList<T>` or `DoublyLinkedList<T>`, they can be added reversed.

```csharp
var list = new DoublyLinkedList<int>(new[] {1, 2, 3, 4});
var anotherList = new DoublyLinkedList<int>(new[] {5, 6, 7, 8});
list.AddFirst(anotherList, true);
foreach (var item in list.Forward)
	Console.Write(item);
```

outputs

```csharp
87651234
```

## `AddFirstFrom()` and `AddLastFrom()`

These methods perform the addition of a `DoublyLinkedList<T>` but moving the nodes into the other `DoublyLinkedList<T>`. No memory allocations and copies of values are performed.

The `DoublyLinkedList<T>` passed as parameter becomes empty.

## `Append()` 

A static method that returns a `DoublyLinkedList<T>` instance with the items of both lists appended. 

```csharp
var list = DoublyLinkedList.Append(
	new DoublyLinkedList<int>(new[] {1, 2, 3, 4}), 
	new DoublyLinkedList<int>(new[] {5, 6, 7, 8}));
foreach (var item in list.Forward)
	Console.Write(item);
```

outputs

```
12345678
```

## `Clone()` 

Returns a shallow clone of the a `DoublyLinkedList<T>`. 

```csharp
var list = new DoublyLinkedList<int>(new[] {1, 2, 3, 4});
var clone = list.Clone();
foreach (var item in list.Forward)
	Console.Write(item);
```

outputs

```
1234
```

## `Reverse()` 

Returns a shallow clone of the a `DoublyLinkedList<T>` with the items in reversed order.

```csharp
var list = new DoublyLinkedList<int>(new[] {1, 2, 3, 4});
var reversed = list.Reverse();
foreach (var item in reversed.Forward)
	Console.Write(item);
```

outputs

```
4321
```

## `ReverseInPlace()` 

`ReverseInPlace()` reverses the list items in-place. It flips the internal node references with no memory allocations.

```csharp
var list = new DoublyLinkedList<int>(new[] {1, 2, 3, 4});
list.ReverseInPlace();
foreach (var item in list.Forward)
	Console.Write(item);
```

also outputs

```
4321
```

## 