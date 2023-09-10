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
        public readonly struct WhereReverseEnumeration
            : IValueEnumerable<T, WhereReverseEnumeration.DisposableEnumerator>
        {
            readonly DoublyLinkedList<T> list;
            readonly Func<T, bool> predicate;

            internal WhereReverseEnumeration(DoublyLinkedList<T> list, Func<T, bool> predicate)
            {
                this.list = list;
                this.predicate = predicate;
            }

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator GetEnumerator() => 
                new(list, predicate);

            DisposableEnumerator IValueEnumerable<T, WhereReverseEnumeration.DisposableEnumerator>.GetEnumerator() =>
                new(list, predicate);

            IEnumerator<T> IEnumerable<T>.GetEnumerator() =>
                new DisposableEnumerator(list, predicate);

            IEnumerator IEnumerable.GetEnumerator() =>
                new DisposableEnumerator(list, predicate);

            public struct Enumerator
            {
                readonly DoublyLinkedList<T> list;
                readonly Func<T, bool> predicate;
                readonly int version;
                Node? current;

                internal Enumerator(DoublyLinkedList<T> list, Func<T, bool> predicate)
                {
                    this.list = list;
                    this.predicate = predicate;
                    version = list.version;
                    current = null;
                }

                public readonly T Current
                    => current!.Value;

                public bool MoveNext()
                {
                    if (version != list.version) 
                        Throw.InvalidOperationException();
                    do
                    {
                        current = current is null 
                            ? list.Last 
                            : current.Previous;
                    }
                    while (current is not null && !predicate(current.Value));
                    return current is not null;
                }
            }

            public struct DisposableEnumerator 
                : IEnumerator<T>
            {
                readonly DoublyLinkedList<T> list;
                readonly Func<T, bool> predicate;
                readonly int version;
                Node? current;

                internal DisposableEnumerator(DoublyLinkedList<T> list, Func<T, bool> predicate)
                {
                    this.list = list;
                    this.predicate = predicate;
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
                    do
                    {
                        current = current is null 
                            ? list.Last 
                            : current.Previous;
                    }
                    while (current is not null && !predicate(current.Value));
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
