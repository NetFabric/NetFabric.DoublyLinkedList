using NetFabric.Hyperlinq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace NetFabric
{
    public partial class DoublyLinkedList<T>
    {
        public readonly struct ForwardEnumeration 
            : IValueReadOnlyList<T, ForwardEnumeration.DisposableEnumerator>
        {
            readonly DoublyLinkedList<T> list;

            internal ForwardEnumeration(DoublyLinkedList<T> list)
                => this.list = list;

            public WhereForwardEnumeration Where(Func<T, bool> predicate)
                => new(list, predicate);  

            public int Count =>
                list.count;
            
            public T this[int index]
            {
                get
                {
                    if ((uint)index >= (uint)Count) 
                        Throw.ArgumentOutOfRangeException(nameof(index));
                    return index < Count / 2 
                        ? ForwardOffset(list.First, index)!.Value 
                        : ReverseOffset(list.Last, Count - index - 1)!.Value;
                }
            }

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator GetEnumerator() => 
                new(list);

            DisposableEnumerator IValueEnumerable<T, DisposableEnumerator>.GetEnumerator() =>
                new(list);

            IEnumerator<T> IEnumerable<T>.GetEnumerator() =>
                new DisposableEnumerator(list);

            IEnumerator IEnumerable.GetEnumerator() =>
                new DisposableEnumerator(list);

            public struct Enumerator
            {
                readonly DoublyLinkedList<T> list;
                readonly int version;
                Node? current;

                internal Enumerator(DoublyLinkedList<T> list)
                {
                    this.list = list;
                    version = list.version;
                    current = null;
                }

                public readonly T Current
                    => current!.Value;

                public bool MoveNext()
                {
                    if (version != list.version) 
                        Throw.InvalidOperationException();
                    current = current is null 
                        ? list.First 
                        : current.Next;
                    return current is not null;
                }
            }

            public struct DisposableEnumerator 
                : IEnumerator<T>
            {
                readonly DoublyLinkedList<T> list;
                readonly int version;
                Node? current;

                internal DisposableEnumerator(DoublyLinkedList<T> list)
                {
                    this.list = list;
                    version = list.version;
                    current = null;
                }

                public readonly T Current
                    => current!.Value;
                readonly object? IEnumerator.Current 
                    => current!.Value;

                public bool MoveNext()
                {
                    if (version != list.version) Throw.InvalidOperationException();
                    current = current is null 
                        ? list.First 
                        : current.Next;
                    return current is not null;
                }

                public void Reset() 
                    => current = null;

                public readonly void Dispose() 
                { }
            }  
        }
    }
}
