using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric
{
    public sealed partial class DoubleLinkedList<T>
    {
        internal Node head;
        internal Node tail;
        int count;
        int version;

        public DoubleLinkedList()
        {
            head = null;
            tail = null;
            count = 0;
            version = 0;
        }

        public DoubleLinkedList(IEnumerable<T> enumerable)
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));

            (head, tail, count) = GetNodes(enumerable, this);
            version = 0;
        }

        public DoubleLinkedList(IReadOnlyList<T> collection, bool reversed = false)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            if (collection.Count == 0)
                return;

            if (reversed)
                (head, tail) = GetNodesReversed(collection, this);
            else
                (head, tail) = GetNodes(collection, this);

            count = collection.Count;
            version = 0;
        }

        public DoubleLinkedList(DoubleLinkedList<T> list, bool reversed = false)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count == 0)
                return;

            if (reversed)
                (head, tail) = GetNodesReversed(list, this);
            else
                (head, tail) = GetNodes(list, this);

            count = list.Count;
            version = 0;
        }

        public Node First =>
            head;

        public Node Last =>
            tail;

        public int Count =>
            count;

        internal int Version =>
            version;

        public bool IsEmpty =>
            head is null;

        void ValidateNode(Node node)
        {
            if (node is null)
                throw new ArgumentNullException(nameof(node));

            if (node.List != this)
                throw new InvalidOperationException();
        }

        public Node AddAfter(Node node, T value)
        {
            ValidateNode(node);
            var result = new Node
            {
                List = this,
                Value = value,
                Next = node.Next,
                Previous = node,
            };
            if (tail == node)
                tail = result;
            else
                node.Next.Previous = result;
            node.Next = result;
            count++;
            version++;
            return result;
        }

        public Node AddBefore(Node node, T value)
        {
            ValidateNode(node);
            var result = new Node
            {
                List = this,
                Value = value,
                Next = node,
                Previous = node.Previous,
            };
            if (head == node)
                head = result;
            else
                node.Previous.Next = result;
            node.Previous = result;
            count++;
            version++;
            return result;
        }

        public Node AddFirst(T value)
        {
            var result = new Node
            {
                List = this,
                Value = value,
                Next = head,
                Previous = null,
            };
            if (IsEmpty)
                tail = result;
            else
                head.Previous = result;
            head = result;
            count++;
            version++;
            return result;
        }

        public void AddFirst(IEnumerable<T> collection)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            var (tempHead, tempTail, tempCount) = GetNodes(collection, this);

            if (tempHead is null)
                return;

            if (IsEmpty)
            {
                head = tempHead;
                tail = tempTail;
            }
            else
            {
                head.Previous = tempTail;
                tempTail.Next = head;
                head = tempHead;
            }
            count += tempCount;
            version++;
        }

        public void AddFirst(IReadOnlyList<T> collection, bool reversed = false)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            if (collection.Count == 0)
                return;

            Node tempHead;
            Node tempTail;
            if (reversed)
                (tempHead, tempTail) = GetNodesReversed(collection, this);
            else
                (tempHead, tempTail) = GetNodes(collection, this);

            if (IsEmpty)
            {
                head = tempHead;
                tail = tempTail;
            }
            else
            {
                head.Previous = tempTail;
                tempTail.Next = head;
                head = tempHead;
            }
            count += collection.Count;
            version++;
        }

        public void AddFirst(DoubleLinkedList<T> list, bool reversed = false)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count == 0)
                return;

            Node tempHead;
            Node tempTail;
            if (reversed)
                (tempHead, tempTail) = GetNodesReversed(list, this);
            else
                (tempHead, tempTail) = GetNodes(list, this);

            if (IsEmpty)
            {
                head = tempHead;
                tail = tempTail;
            }
            else
            {
                head.Previous = tempTail;
                tempTail.Next = head;
                head = tempHead;
            }
            count += list.count;
            version++;
        }

        public void AddFirstFrom(DoubleLinkedList<T> list, bool reversed = false)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count == 0)
                return;

            Node tempHead = null;
            Node tempTail = null;
            if (reversed)
                (tempHead, tempTail) = AssignReversed(list, this);
            else
                (tempHead, tempTail) = Assign(list, this);

            if (IsEmpty)
            {
                head = tempHead;
                tail = tempTail;
            }
            else
            {
                head.Previous = tempTail;
                tempTail.Next = head;
                head = tempHead;
            }
            count += list.count;
            version++;
            list.Invalidate();
        }

        public Node AddLast(T value)
        {
            var result = new Node
            {
                List = this,
                Value = value,
                Next = null,
                Previous = tail,
            };
            if (IsEmpty)
                head = result;
            else
                tail.Next = result;
            tail = result;
            count++;
            version++;
            return result;
        }

        public void AddLast(IEnumerable<T> collection)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            var (tempHead, tempTail, tempCount) = GetNodes(collection, this);

            if (tempHead is null)
                return;

            if (IsEmpty)
            {
                head = tempHead;
                tail = tempTail;
            }
            else
            {
                tail.Next = tempHead;
                tempHead.Previous = tail;
                tail = tempTail;
            }
            count += tempCount;
            version++;
        }

        public void AddLast(IReadOnlyList<T> collection, bool reversed = false)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            if (collection.Count == 0)
                return;

            Node tempHead;
            Node tempTail;
            if (reversed)
                (tempHead, tempTail) = GetNodesReversed(collection, this);
            else
                (tempHead, tempTail) = GetNodes(collection, this);

            if (IsEmpty)
            {
                head = tempHead;
                tail = tempTail;
            }
            else
            {
                tail.Next = tempHead;
                tempHead.Previous = tail;
                tail = tempTail;
            }
            count += collection.Count;
            version++;
        }

        public void AddLast(DoubleLinkedList<T> list, bool reversed = false)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count == 0)
                return;

            Node tempHead;
            Node tempTail;
            if (reversed)
                (tempHead, tempTail) = GetNodesReversed(list, this);
            else
                (tempHead, tempTail) = GetNodes(list, this);

            if (IsEmpty)
            {
                head = tempHead;
                tail = tempTail;
            }
            else
            {
                tail.Next = tempHead;
                tempHead.Previous = tail;
                tail = tempTail;
            }
            count += list.count;
            version++;
        }

        public void AddLastFrom(DoubleLinkedList<T> list, bool reversed = false)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count == 0)
                return;

            Node tempHead = null;
            Node tempTail = null;
            if (reversed)
                (tempHead, tempTail) = AssignReversed(list, this);
            else
                (tempHead, tempTail) = Assign(list, this);

            if (IsEmpty)
            {
                head = tempHead;
                tail = tempTail;
            }
            else
            {
                tail.Next = tempHead;
                tempHead.Previous = tail;
                tail = tempTail;
            }
            count += list.count;
            version++;
            list.Invalidate();
        }

        public void Clear()
        {
            var current = head;
            while (!(current is null))
            {
                var temp = current;
                current = current.Next;
                temp.Invalidate();
            }
            Invalidate();
        }

        internal void Invalidate()
        {
            head = null;
            tail = null;
            count = 0;
            version++;
        }

        public Node Find(T value)
        {
            var node = head;
            if (value == null)
            {
                while (!(node is null))
                {
                    if (node.Value == null)
                        return node;

                    node = node.Next;
                }
            }
            else
            {
                var comparer = EqualityComparer<T>.Default;
                while (!(node is null))
                {
                    if (comparer.Equals(node.Value, value))
                        return node;

                    node = node.Next;
                }
            }
            return null;
        }

        public Node FindLast(T value)
        {
            var node = tail;
            if (value == null)
            {
                while (!(node is null))
                {
                    if (node.Value == null)
                        return node;

                    node = node.Previous;
                }
            }
            else
            {
                var comparer = EqualityComparer<T>.Default;
                while (!(node is null))
                {
                    if (comparer.Equals(node.Value, value))
                        return node;

                    node = node.Previous;
                }
            }
            return null;
        }

        public ForwardEnumeration EnumerateForward() =>
            new ForwardEnumeration(this);

        public ReverseEnumeration EnumerateReversed() =>
            new ReverseEnumeration(this);

        public bool Remove(T value)
        {
            var node = Find(value);
            if (node is null)
                return false;

            if (head == node && tail == node)
            {
                head = null;
                tail = null;
            }
            else
            {
                if (head == node)
                {
                    head = node.Next;
                    node.Next.Previous = null;
                }
                else
                {
                    node.Previous.Next = node.Next;
                }

                if (tail == node)
                {
                    tail = node.Previous;
                    node.Previous.Next = null;
                }
                else
                {
                    node.Next.Previous = node.Previous;
                }
            }
            node.Invalidate();
            count--;
            version++;
            return true;
        }

        public bool RemoveLast(T value)
        {
            var node = FindLast(value);
            if (node is null)
                return false;

            if (head == node && tail == node)
            {
                head = null;
                tail = null;
            }
            else
            {
                if (head == node)
                {
                    head = node.Next;
                    node.Next.Previous = null;
                }
                else
                {
                    node.Previous.Next = node.Next;
                }

                if (tail == node)
                {
                    tail = node.Previous;
                    node.Previous.Next = null;
                }
                else
                {
                    node.Next.Previous = node.Previous;
                }
            }
            node.Invalidate();
            count--;
            version++;
            return true;
        }

        public void RemoveFirst()
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            var node = head;
            if (tail == node)
            {
                head = null;
                tail = null;
            }
            else
            {
                head = node.Next;
                head.Previous = null;
            }
            node.Invalidate();
            count--;
            version++;
        }

        public void RemoveLast()
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            var node = tail;
            if (head == node)
            {
                head = null;
                tail = null;
            }
            else
            {
                tail = node.Previous;
                tail.Next = null;
            }
            node.Invalidate();
            count--;
            version++;
        }

        public DoubleLinkedList<T> Clone() =>
            new DoubleLinkedList<T>(this, false);

        public DoubleLinkedList<T> Reverse() =>
            new DoubleLinkedList<T>(this, true);

        public void ReverseInPlace()
        {
            if (count < 2)
                return;

            Node temp;
            var current = head;
            while (!(current is null))
            {
                temp = current.Next;
                current.Next = current.Previous;
                current.Previous = temp;
                current = temp;
            }
            temp = head;
            head = tail;
            tail = temp;
            version++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (Node Head, Node Tail, int Count) GetNodes(IEnumerable<T> source, DoubleLinkedList<T> target)
        {
            Node head = null;
            Node tail = null;
            var count = 0;
            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    head = tail = new Node
                    {
                        List = target,
                        Value = enumerator.Current,
                        Next = null,
                        Previous = null,
                    };
                    count++;
                    while (enumerator.MoveNext())
                    {
                        var node = new Node
                        {
                            List = target,
                            Value = enumerator.Current,
                            Next = null,
                            Previous = tail,
                        };
                        tail.Next = node;
                        tail = node;
                        count++;
                    }
                }
            }
            return (head, tail, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (Node Head, Node Tail) GetNodes(IReadOnlyList<T> source, DoubleLinkedList<T> target)
        {
            Node head;
            Node tail;
            head = tail = new Node
            {
                List = target,
                Value = source[0],
                Next = null,
                Previous = null,
            };

            for (int index = 1, end = source.Count; index < end; index++)
            {
                var node = new Node
                {
                    List = target,
                    Value = source[index],
                    Next = null,
                    Previous = tail,
                };
                tail.Next = node;
                tail = node;
            }

            return (head, tail);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (Node Head, Node Tail) GetNodesReversed(IReadOnlyList<T> source, DoubleLinkedList<T> target)
        {
            Node head;
            Node tail;
            head = tail = new Node
            {
                List = target,
                Value = source[0],
                Next = null,
                Previous = null,
            };

            for (var index = source.Count - 2; index >= 0; index--)
            {
                var node = new Node
                {
                    List = target,
                    Value = source[index],
                    Next = null,
                    Previous = tail,
                };
                tail.Next = node;
                tail = node;
            }

            return (head, tail);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (Node Head, Node Tail) GetNodes(DoubleLinkedList<T> source, DoubleLinkedList<T> target)
        {
            Node head;
            Node tail;
            var current = source.head;
            head = tail = new Node
            {
                List = target,
                Value = current.Value,
                Next = null,
                Previous = null,
            };

            current = current.Next;
            while (!(current is null))
            {
                var node = new Node
                {
                    List = target,
                    Value = current.Value,
                    Next = null,
                    Previous = tail,
                };
                tail.Next = node;
                tail = node;

                current = current.Next;
            }

            return (head, tail);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (Node Head, Node Tail) GetNodesReversed(DoubleLinkedList<T> source, DoubleLinkedList<T> target)
        {
            Node head;
            Node tail;
            var current = source.head;
            head = tail = new Node
            {
                List = target,
                Value = current.Value,
                Next = null,
                Previous = null,
            };

            current = current.Next;
            while (!(current is null))
            {
                var node = new Node
                {
                    List = target,
                    Value = current.Value,
                    Next = head,
                    Previous = null,
                };
                head.Previous = node;
                head = node;

                current = current.Next;
            }

            return (head, tail);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (Node Head, Node Tail) Assign(DoubleLinkedList<T> source, DoubleLinkedList<T> target)
        {
            var current = source.head;
            while (!(current is null))
            {
                current.List = target;

                current = current.Next;
            }
            return (source.head, source.tail);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (Node Head, Node Tail) AssignReversed(DoubleLinkedList<T> source, DoubleLinkedList<T> target)
        {
            Node head;
            Node tail;
            var current = source.head;
            var next = current.Next;

            current.List = target;
            current.Next = null;
            current.Previous = null;

            head = tail = current;

            current = next;
            while (!(current is null))
            {
                next = current.Next;

                current.List = target;
                current.Next = head;
                current.Previous = null;
                head.Previous = current;
                head = current;

                current = next;
            }

            return (head, tail);
        }
    }
}
