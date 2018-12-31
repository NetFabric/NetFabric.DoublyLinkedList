using System;
using System.Collections.Generic;

namespace NetFabric
{
    public partial class DoublyLinkedList<T>
    {
        internal Node head;
        internal Node tail;
        int count;
        int version;

        public DoublyLinkedList()
        {
            head = null;
            tail = null;
            count = 0;
            version = 0;
        }

        public DoublyLinkedList(IEnumerable<T> collection) :
            this()
        {
            AddLast(collection);
        }

        public DoublyLinkedList(IReadOnlyList<T> collection, bool reversed = false) :
            this()
        {
            AddLast(collection, reversed);
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
            version++;
        }

        public void AddFirst(IReadOnlyList<T> collection, bool reversed = false)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            if (collection.Count == 0)
                return;

            Node tempHead = null;
            Node tempTail = null;
            tempHead = tempTail = new Node
            {
                List = this,
                Value = collection[0],
                Next = null,
                Previous = null,
            };
            if (reversed)
                AssignReversed();
            else
                Assign();

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

            void Assign()
            {
                for (int index = 1, end = collection.Count; index < end; index++)
                {
                    var node = new Node
                    {
                        List = this,
                        Value = collection[index],
                        Next = null,
                        Previous = tempTail,
                    };
                    tempTail.Next = node;
                    tempTail = node;
                }
            }

            void AssignReversed()
            {
                for (var index = collection.Count - 2; index >= 0; index--)
                {
                    var node = new Node
                    {
                        List = this,
                        Value = collection[index],
                        Next = null,
                        Previous = tempTail,
                    };
                    tempTail.Next = node;
                    tempTail = node;
                }
            }
        }

        public void AddFirst(DoublyLinkedList<T> list, bool reversed = false)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count == 0)
                return;

            Node tempHead;
            Node tempTail;
            var current = list.head;
            tempHead = tempTail = new Node
            {
                List = this,
                Value = current.Value,
                Next = null,
                Previous = null,
            };

            if (reversed)
                AssignReversed();
            else
                Assign();

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

            void Assign()
            {
                current = current.Next;
                while (!(current is null))
                {
                    var node = new Node
                    {
                        List = this,
                        Value = current.Value,
                        Next = null,
                        Previous = tempTail,
                    };
                    tempTail.Next = node;
                    tempTail = node;

                    current = current.Next;
                }
            }

            void AssignReversed()
            {
                current = current.Next;
                while (!(current is null))
                {
                    var node = new Node
                    {
                        List = this,
                        Value = current.Value,
                        Next = tempHead,
                        Previous = null,
                    };
                    tempHead.Previous = node;
                    tempHead = node;

                    current = current.Next;
                }
            }
        }

        public void AddFirstFrom(DoublyLinkedList<T> list, bool reversed = false)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count == 0)
                return;

            Node tempHead = null;
            Node tempTail = null;
            if (reversed)
                AssignReversed();
            else
                Assign();

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

            void Assign()
            {
                var current = list.head;
                while (!(current is null))
                {
                    current.List = this;

                    current = current.Next;
                }
                tempHead = list.head;
                tempTail = list.tail;
            }

            void AssignReversed()
            {
                var current = list.head;
                var next = current.Next;

                current.List = this;
                current.Next = null;
                current.Previous = null;
                tempHead = tempTail = current;

                current = next;
                while (!(current is null))
                {
                    next = current.Next;

                    current.List = this;
                    current.Next = tempHead;
                    current.Previous = null;
                    tempHead.Previous = current;
                    tempHead = current;

                    current = next;
                }
            }
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
            version++;
        }

        public void AddLast(IReadOnlyList<T> collection, bool reversed = false)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            if (collection.Count == 0)
                return;

            Node tempHead = null;
            Node tempTail = null;
            tempHead = tempTail = new Node
            {
                List = this,
                Value = collection[0],
                Next = null,
                Previous = null,
            };
            if (reversed)
                AssignReversed();
            else
                Assign();

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

            void Assign()
            {
                for (int index = 1, end = collection.Count; index < end; index++)
                {
                    var node = new Node
                    {
                        List = this,
                        Value = collection[index],
                        Next = null,
                        Previous = tempTail,
                    };
                    tempTail.Next = node;
                    tempTail = node;
                }
            }

            void AssignReversed()
            {
                for (var index = collection.Count - 2; index >= 0; index--)
                {
                    var node = new Node
                    {
                        List = this,
                        Value = collection[index],
                        Next = null,
                        Previous = tempTail,
                    };
                    tempTail.Next = node;
                    tempTail = node;
                }
            }
        }

        public void AddLast(DoublyLinkedList<T> list, bool reversed = false)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count == 0)
                return;

            Node tempHead = null;
            Node tempTail = null;
            var current = list.head;
            tempHead = tempTail = new Node
            {
                List = this,
                Value = current.Value,
                Next = null,
                Previous = null,
            };

            if (reversed)
                AssignReversed();
            else
                Assign();

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

            void Assign()
            {
                current = current.Next;
                while (!(current is null))
                {
                    var node = new Node
                    {
                        List = this,
                        Value = current.Value,
                        Next = null,
                        Previous = tempTail,
                    };
                    tempTail.Next = node;
                    tempTail = node;

                    current = current.Next;
                }
            }

            void AssignReversed()
            {
                current = current.Next;
                while (!(current is null))
                {
                    var node = new Node
                    {
                        List = this,
                        Value = current.Value,
                        Next = tempHead,
                        Previous = null,
                    };
                    tempHead.Previous = node;
                    tempHead = node;

                    current = current.Next;
                }
            }
        }

        public void AddLastFrom(DoublyLinkedList<T> list, bool reversed = false)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count == 0)
                return;

            Node tempHead = null;
            Node tempTail = null;
            if (reversed)
                AssignReversed();
            else
                Assign();

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

            void Assign()
            {
                var current = list.head;
                while (!(current is null))
                {
                    current.List = this;

                    current = current.Next;
                }
                tempHead = list.head;
                tempTail = list.tail;
            }

            void AssignReversed()
            {
                var current = list.head;
                var next = current.Next;

                current.List = this;
                current.Next = null;
                current.Previous = null;
                tempHead = tempTail = current;

                current = next;
                while (!(current is null))
                {
                    next = current.Next;

                    current.List = this;
                    current.Next = tempHead;
                    current.Previous = null;
                    tempHead.Previous = current;
                    tempHead = current;

                    current = next;
                }
            }
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

        public DoublyLinkedList<T> Clone()
        {
            var list = new DoublyLinkedList<T>
            {
                head = null,
                tail = null,
                count = count,
                version = 0,
            };

            var current = head;
            if (!(current is null))
            {
                list.head = list.tail = new Node
                {
                    List = list,
                    Value = current.Value,
                    Next = null,
                    Previous = null,
                };
                current = current.Next;
                while (!(current is null))
                {
                    var node = new Node
                    {
                        List = list,
                        Value = current.Value,
                        Next = null,
                        Previous = list.tail,
                    };
                    list.tail.Next = node;
                    list.tail = node;
                    current = current.Next;
                }
            }

            return list;
        }

        public DoublyLinkedList<T> Reverse()
        {
            var list = new DoublyLinkedList<T>
            {
                head = null,
                tail = null,
                count = count,
                version = 0,
            };

            var current = head;
            if (!(current is null))
            {
                list.head = list.tail = new Node
                {
                    List = list,
                    Value = current.Value,
                    Next = null,
                    Previous = null,
                };
                current = current.Next;
                while (!(current is null))
                {
                    var node = new Node
                    {
                        List = list,
                        Value = current.Value,
                        Next = list.head,
                        Previous = null,
                    };
                    list.head.Previous = node;
                    list.head = node;
                    current = current.Next;
                }
            }

            return list;
        }

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
    }
}
