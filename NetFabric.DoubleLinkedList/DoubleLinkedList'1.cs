using System;
using System.Collections.Generic;

namespace NetFabric
{
    public sealed partial class DoubleLinkedList<T>
    {
        public readonly static DoubleLinkedList<T> Empty = new DoubleLinkedList<T>();
        
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

        public DoubleLinkedList(IEnumerable<T> collection) :
            this()
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            AddLast(collection);
        }

        public Node First =>
            head;

        public Node Last =>
            tail;

        public int Count =>
            count;

        internal int Version =>
            version;

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
            if (tail is null)
                tail = result;
            if (!(head is null))
                head.Previous = result;
            head = result;
            count++;
            version++;
            return result;
        }

        public void AddFirst(IEnumerable<T> collection)
        {
            Node tempHead = null;
            Node tempTail = null;
            using (var enumerator = collection.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    tempHead = tempTail = new Node
                    {
                        List = this,
                        Value = enumerator.Current,
                        Next = null,
                        Previous = null,
                    };
                    count++;
                    while (enumerator.MoveNext())
                    {
                        var node = new Node
                        {
                            List = this,
                            Value = enumerator.Current,
                            Next = null,
                            Previous = tempTail,
                        };
                        tempTail.Next = node;
                        tempTail = node;
                        count++;
                    }
                }
            }

            if (!(tempHead is null))
            {
                if (head is null)
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
            }
            version++;
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
            if (head is null)
                head = result;
            if (!(tail is null))
                tail.Next = result;
            tail = result;
            count++;
            version++;
            return result;
        }

        public void AddLast(IEnumerable<T> collection)
        {
            Node tempHead = null;
            Node tempTail = null;
            using (var enumerator = collection.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    tempHead = tempTail = new Node
                    {
                        List = this,
                        Value = enumerator.Current,
                        Next = null,
                        Previous = null,
                    };
                    count++;
                    while (enumerator.MoveNext())
                    {
                        var node = new Node
                        {
                            List = this,
                            Value = enumerator.Current,
                            Next = null,
                            Previous = tempTail,
                        };
                        tempTail.Next = node;
                        tempTail = node;
                        count++;
                    }
                }
            }

            if (!(tempHead is null))
            {
                if (tail is null)
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
            }
            version++;
        }

        public void Clear()
        {
            var current = head;
            while (!(current is null))
            {
                Node temp = current;
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
            if (Object.ReferenceEquals(value, null))
            {
                while (!(node is null))
                {
                    if (Object.ReferenceEquals(node.Value, null))
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
            if (Object.ReferenceEquals(value, null))
            {
                while (!(node is null))
                {
                    if (Object.ReferenceEquals(node.Value, null))
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
            if (head is null)
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
            if (tail is null)
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
            new DoubleLinkedList<T>(this.EnumerateForward());

        public DoubleLinkedList<T> Reverse() =>
            new DoubleLinkedList<T>(this.EnumerateReversed());

        public void ReverseInPlace()
        {
            if(count < 2)
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
    }
}
