# NetFabric.DoubleLinkedList

An alternative to the [`System.Collections.Generic.LinkedList<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1) with reverse operation and enumeration without allocation.

The public API is very similar but the internals are very different with a more efficient implementation.

The public API has the following changes:

## Forward and reverse enumeration

`DoubleLinkedList<T>` does not  implement `IEnumerable<T>`. Call the methods `EnumerateForward()` or `EnumerateReversed()` to get an enumerator that goes in the direction you require without performing any changes to the list.

```csharp
var list = new DoubleLinkedList<int>(new[] {1, 2, 3, 4});
foreach (var item in list.EnumerateReversed())
	Console.WriteLine(item);
```
outputs
```
4
3
2
1
```

These can also be used with [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/):

```csharp
var list = new DoubleLinkedList<int>(new[] {1, 2, 3, 4});
Console.WriteLine(list.EnumerateReversed().First());
```
outputs
```
4
```

## `Reverse()` and `ReverseInPlace()` methods

`Reverse()` returns another `DoubleLinkedList<T>` instance with the same items but in reverse order. 

```csharp
var list = new DoubleLinkedList<int>(new[] {1, 2, 3, 4});
var reversed = list.Reverse();
foreach (var item in reversed.EnumerateForward())
	Console.WriteLine(item);
```
outputs
```
4
3
2
1
```

`ReverseInPlace()` reverses the list items in-place. It flips the internal node references with no memory allocations.

```csharp
var list = new DoubleLinkedList<int>(new[] {1, 2, 3, 4});
list.ReverseInPlace();
foreach (var item in list.EnumerateForward())
	Console.WriteLine(item);
```
outputs
```
4
3
2
1
```
