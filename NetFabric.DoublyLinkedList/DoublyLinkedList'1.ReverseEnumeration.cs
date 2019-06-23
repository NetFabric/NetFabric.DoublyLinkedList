using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric
{
    public partial class DoublyLinkedList<T>
    {
        public readonly struct ReverseEnumeration : IReadOnlyCollection<T>
        {
            readonly DoublyLinkedList<T> list;

            internal ReverseEnumeration(DoublyLinkedList<T> list)
            {
                this.list = list;
            }

            public int Count =>
                list.count;

            public Enumerator GetEnumerator() => 
                new Enumerator(list);

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => 
                new Enumerator(list);

            IEnumerator IEnumerable.GetEnumerator() => 
                new Enumerator(list);

            public struct Enumerator : IEnumerator<T>
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
                    current = list.tail;
                    if (list.IsEmpty)
                        state = State.Empty;
                    else
                        state = State.First;
                }

                public T Current => 
                    current.Value;

                object IEnumerator.Current => 
                    current.Value;

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
                            state = State.Normal;
                            return true;
                        default:
                            return false;
                    }

                    void ThrowInvalidOperation() => throw new InvalidOperationException();
                }

                public void Reset()
                {
                    if (version != list.version)
                        ThrowInvalidOperation();

                    current = list.tail;
                    if (list.IsEmpty)
                        state = State.Empty;
                    else
                        state = State.First;

                    void ThrowInvalidOperation() => throw new InvalidOperationException();
                }

                public void Dispose()
                {
                    // nothing to do
                }
            }
        }
    }
}
