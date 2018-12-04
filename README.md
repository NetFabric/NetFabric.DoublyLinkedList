# NetFabric.DoubleLinkedList

An alternative to the [`System.Collections.Generic.LinkedList<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1) with reverse operation and enumeration without allocation.

The public API is very similar but the internals are very different with a more efficient implementation. 

New overrides and methods were added to minimizing the memory allocations, number of loops, conditions and assignments required in multiple scenarios.

## Benchmarks

![constructor performance](https://user-images.githubusercontent.com/534533/49405396-38f4ad80-f74a-11e8-9162-22abd8fc4c00.png)

![forward enumeration performance](https://user-images.githubusercontent.com/534533/49405413-40b45200-f74a-11e8-9b63-4ac67efd144d.png)

![reverse enumeration performance](https://user-images.githubusercontent.com/534533/49405544-a43e7f80-f74a-11e8-9aba-544cb3141e33.png)

## Forward and reverse enumeration

`DoubleLinkedList<T>` does not  implement `IEnumerable<T>`. Call the methods `EnumerateForward()` or `EnumerateReversed()` to get an enumerator that goes in the direction you require without performing any changes to the list.

```csharp
var list = new DoubleLinkedList<int>(new[] {1, 2, 3, 4});
foreach (var item in list.EnumerateReversed())
	Console.Write(item);
```
outputs
```
4321
```

These can also be used with [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/):

```csharp
var list = new DoubleLinkedList<int>(new[] {1, 2, 3, 4});
Console.Write(list.EnumerateReversed().First());
```
outputs
```
4
```

## `AddFirst()` and `AddLast()`

These methods now support the addition of a `IEnumerable<T>` or another `DoubleLinkedList<T>`. 

When case the parameter is a `DoubleLinkedList<T>`, it can be added reversed.

```csharp
var list = new DoubleLinkedList<int>(new[] {1, 2, 3, 4});
var anotherList = new DoubleLinkedList<int>(new[] {5, 6, 7, 8});
list.AddFirst(anotherList, true);
foreach (var item in list.EnumerateForward())
	Console.Write(item);
```

outputs

```csharp
87651234
```

## `AddFirstFrom()` and `AddLastFrom()`

These methods perform the addition of a `DoubleLinkedList<T>` but moving the nodes into the other `DoubleLinkedList<T>`. No memory allocations and copies are performed.

The `DoubleLinkedList<T>` passed as parameter becomes empty.

## `Append()` 

A static method that returns a `DoubleLinkedList<T>` instance with the items of both lists appended. 

```csharp
var list = DoubleLinkedList.Append(
	new DoubleLinkedList<int>(new[] {1, 2, 3, 4}), 
	new DoubleLinkedList<int>(new[] {5, 6, 7, 8}));
foreach (var item in list.EnumerateForward())
	Console.Write(item);
```

outputs

```
12345678
```

## `Clone()` 

Returns a shallow clone of the a `DoubleLinkedList<T>`. 

```csharp
var list = new DoubleLinkedList<int>(new[] {1, 2, 3, 4});
var clone = list.Clone();
foreach (var item in list.EnumerateForward())
	Console.Write(item);
```

outputs

```
1234
```

## `Reverse()` 

Returns a shallow clone of the a `DoubleLinkedList<T>` with the items in reversed order.

```csharp
var list = new DoubleLinkedList<int>(new[] {1, 2, 3, 4});
var reversed = list.Reverse();
foreach (var item in reversed.EnumerateForward())
	Console.Write(item);
```

outputs

```
4321
```

## `ReverseInPlace()` 

`ReverseInPlace()` reverses the list items in-place. It flips the internal node references with no memory allocations.

```csharp
var list = new DoubleLinkedList<int>(new[] {1, 2, 3, 4});
list.ReverseInPlace();
foreach (var item in list.EnumerateForward())
	Console.Write(item);
```

also outputs

```
4321
```

## 