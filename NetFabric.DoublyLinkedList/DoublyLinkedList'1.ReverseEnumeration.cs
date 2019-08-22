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
        public readonly struct ReverseEnumeration : IValueReadOnlyCollection<T, ReverseEnumeration.Enumerator>
        {
            readonly DoublyLinkedList<T> list;

            internal ReverseEnumeration(DoublyLinkedList<T> list)
            {
                this.list = list;
            }

            public int Count =>
                list.count;

            [Pure]
            public readonly Enumerator GetEnumerator() => 
                new Enumerator(list);

            readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() =>
                ValueEnumerator.ToEnumerator<T, Enumerator>(new Enumerator(list));

            readonly IEnumerator IEnumerable.GetEnumerator() =>
                ValueEnumerator.ToEnumerator<T, Enumerator>(new Enumerator(list));

            public struct Enumerator : IValueEnumerator<T>
            {
                enum State
                {
                    Normal,
                    First,
                    Empty,
                }

                readonly DoublyLinkedList<T> list;
                readonly int version;
                Node current;
                State state;

                internal Enumerator(DoublyLinkedList<T> list)
                {
                    this.list = list;
                    version = list.version;
                    current = null;
                    state = list.IsEmpty ? State.Empty : State.First;
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
                        case State.Normal:
                            current = current.Previous;
                            return current is object;
                        case State.First:
                            current = list.tail;
                            state = State.Normal;
                            return true;
                        default:
                            return false;
                    }

                    static void ThrowInvalidOperation() => throw new InvalidOperationException();
                }
            }
        }
    }
}
