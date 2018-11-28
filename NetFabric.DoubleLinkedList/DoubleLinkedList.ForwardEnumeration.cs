using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric
{
    public sealed partial class DoubleLinkedList<T>
    {
        public readonly struct ForwardEnumeration : IReadOnlyCollection<T>
        {
            readonly DoubleLinkedList<T> list;

            internal ForwardEnumeration(DoubleLinkedList<T> list)
            {
                this.list = list;
            }

            public int Count =>
                list.count;

            public Enumerator GetEnumerator() => 
                new Enumerator(list);

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => 
                GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => 
                GetEnumerator();

            public struct Enumerator : IEnumerator<T>
            {
                readonly DoubleLinkedList<T> list;
                readonly int version;
                Node current;

                internal Enumerator(DoubleLinkedList<T> list)
                {
                    this.list = list;
                    version = list.version;
                    current = null;
                }

                public T Current => 
                    current.Value;

                object IEnumerator.Current => 
                    Current;

                public bool MoveNext()
                {
                    if (version != list.version)
                        throw new InvalidOperationException();

                    if (current is null)
                    {
                        current = list.head;
                        return !(current is null);
                    }

                    if (current.Next is null)
                        return false;

                    current = current.Next;
                    return true;
                }

                public void Reset()
                {
                    if (version != list.version)
                        throw new InvalidOperationException();

                    current = null;
                }

                public void Dispose()
                {
                    // nothing to do
                }
            }
        }
    }
}
