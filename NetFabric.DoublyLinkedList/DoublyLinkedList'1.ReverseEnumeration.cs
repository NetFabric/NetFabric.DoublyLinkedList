using NetFabric.Hyperlinq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace NetFabric
{
    public partial class DoublyLinkedList<T>
    {
        public readonly struct ReverseEnumeration 
            : IValueReadOnlyList<T, ReverseEnumeration.DisposableEnumerator>
        {
            readonly DoublyLinkedList<T> list;

            internal ReverseEnumeration(DoublyLinkedList<T> list)
                => this.list = list;

            public int Count =>
                list.count;
            
            public T this[int index]
            {
                get
                {
                    if ((uint)index >= (uint)Count) 
                        Throw.ArgumentOutOfRangeException(nameof(index));
                    return index < Count / 2 
                        ? ReverseOffset(list.Last, index)!.Value 
                        : ForwardOffset(list.First, Count - index - 1)!.Value;
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
                    if (version != list.version) Throw.InvalidOperationException();
                    current = current is null 
                        ? list.Last 
                        : current.Previous;
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
                    if (version != list.version) 
                        Throw.InvalidOperationException();
                    current = current is null 
                        ? list.Last 
                        : current.Previous;
                    return current is not null;
                }

                public readonly void Reset() 
                    => Throw.NotSupportedException();

                public readonly void Dispose() 
                { }
            }
        }
    }
}
