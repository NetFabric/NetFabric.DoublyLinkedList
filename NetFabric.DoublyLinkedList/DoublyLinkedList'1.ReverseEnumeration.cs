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
        public readonly struct ReverseEnumeration : IValueReadOnlyCollection<T, ReverseEnumeration.DisposableEnumerator>
        {
            readonly DoublyLinkedList<T> list;

            internal ReverseEnumeration(DoublyLinkedList<T> list)
            {
                this.list = list;
            }

            public int Count =>
                list.count;

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly Enumerator GetEnumerator() =>
                new Enumerator(list);

            readonly DisposableEnumerator IValueEnumerable<T, DisposableEnumerator>.GetEnumerator() =>
                new DisposableEnumerator(list);

            readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() =>
                new DisposableEnumerator(list);

            readonly IEnumerator IEnumerable.GetEnumerator() =>
                new DisposableEnumerator(list);

            public struct Enumerator
            {
                readonly DoublyLinkedList<T> list;
                readonly int version;
                Node current;
                EnumeratorState state;

                internal Enumerator(DoublyLinkedList<T> list)
                {
                    this.list = list;
                    version = list.version;
                    current = null;
                    state = list.IsEmpty ? EnumeratorState.Empty : EnumeratorState.First;
                }

                public readonly T Current
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get => current.Value;
                }

                public bool MoveNext()
                {
                    if (version != list.version)
                        ThrowInvalidOperation();

                    switch (state)
                    {
                        case EnumeratorState.Normal:
                            current = current.Previous;
                            return current is object;
                        case EnumeratorState.First:
                            current = list.tail;
                            state = EnumeratorState.Normal;
                            return true;
                        default:
                            return false;
                    }

                    static void ThrowInvalidOperation() => throw new InvalidOperationException();
                }
            }

            public struct DisposableEnumerator : IEnumerator<T>
            {
                readonly DoublyLinkedList<T> list;
                readonly int version;
                Node current;
                EnumeratorState state;

                internal DisposableEnumerator(DoublyLinkedList<T> list)
                {
                    this.list = list;
                    version = list.version;
                    current = null;
                    state = list.IsEmpty ? EnumeratorState.Empty : EnumeratorState.First;
                }

                public readonly T Current
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get => current.Value;
                }
                readonly object IEnumerator.Current => current.Value;

                public bool MoveNext()
                {
                    if (version != list.version)
                        ThrowInvalidOperation();

                    switch (state)
                    {
                        case EnumeratorState.Normal:
                            current = current.Previous;
                            return current is object;
                        case EnumeratorState.First:
                            current = list.tail;
                            state = EnumeratorState.Normal;
                            return true;
                        default:
                            return false;
                    }

                    static void ThrowInvalidOperation() => throw new InvalidOperationException();
                }

                public readonly void Reset() => throw new NotSupportedException();

                public readonly void Dispose() { }
            }
        }
    }
}
