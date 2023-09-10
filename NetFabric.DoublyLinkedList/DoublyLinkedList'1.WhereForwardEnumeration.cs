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
        public readonly struct WhereForwardEnumeration
            : IValueEnumerable<T, WhereForwardEnumeration.DisposableEnumerator>
        {
            readonly DoublyLinkedList<T> list;
            readonly Func<T, bool> predicate;

            internal WhereForwardEnumeration(DoublyLinkedList<T> list, Func<T, bool> predicate)
            {
                this.list = list;
                this.predicate = predicate;
            }

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator GetEnumerator() => 
                new(list, predicate);

            DisposableEnumerator IValueEnumerable<T, WhereForwardEnumeration.DisposableEnumerator>.GetEnumerator() =>
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
                            ? list.First 
                            : current.Next;
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
                            ? list.First 
                            : current.Next;
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
