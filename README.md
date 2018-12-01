# NetFabric.DoubleLinkedList

An alternative to the [`System.Collections.Generic.LinkedList<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1) with reverse operation and enumeration without allocation.

The public API is very similar but the internals are very different with a more efficient implementation.

The public API has the following changes:

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

## `Reverse()` and `ReverseInPlace()` methods

`Reverse()` returns a `DoubleLinkedList<T>` instance with the same items but in reverse order. 

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

## `Append()` and `AppendInPlace()` methods

`Append()` returns a `DoubleLinkedList<T>` instance with the items of both lists appended. 

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

`AppendInPlace()` also appends the two lists but without memory allocation. It reuses the nodes of the two lists. This also means the two lists are emptied of all its items.

It also has optional boolean parameters that allow the efficient reverse of the input lists while appended.


```csharp
var list = DoubleLinkedList.AppendInPlace(
	new DoubleLinkedList<int>(new[] {1, 2, 3, 4}), 
	new DoubleLinkedList<int>(new[] {5, 6, 7, 8})
	true, false);
foreach (var item in list.EnumerateForward())
	Console.Write(item);
```
outputs
```
43215678
```