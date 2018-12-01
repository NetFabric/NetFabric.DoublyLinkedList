using System;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NetFabric.DoubleLinkedList.Tests")]

namespace NetFabric
{
    public static class DoubleLinkedList
    {
        public static DoubleLinkedList<T> Append<T>(DoubleLinkedList<T> left, DoubleLinkedList<T> right) 
        {
            var result = new DoubleLinkedList<T>();
            result.AddLast(left.EnumerateForward());
            result.AddLast(right.EnumerateForward());
            return result;
        }

        public static DoubleLinkedList<T> AppendInPlace<T>(DoubleLinkedList<T> left, DoubleLinkedList<T> right, bool reverseLeft = false, bool reverseRight = false) 
        {
            var result = new DoubleLinkedList<T>();

            if (reverseLeft && left.Count >= 2)
                AssignAndReverse(left);
            else
                Assign(left);

            if (reverseRight && right.Count >= 2)
                AssignAndReverse(right);
            else
                Assign(right);

            if (left.head is null)
                result.head = right.head;
            else
                result.head = left.head;

            if(!(left.tail is null))
                left.tail.Next = right.head;

            if(!(right.head is null))
                right.head.Previous = left.tail;

            if (right.tail is null)
                result.tail = left.tail;
            else
                result.tail = right.tail;

            left.Invalidate();
            right.Invalidate();

            return result;

            void Assign(DoubleLinkedList<T> list)
            {
                var current = list.head;
                while(!(current is null))
                {
                    current.List = result;
                    current = current.Next;
                }
            }

            void AssignAndReverse(DoubleLinkedList<T> list)
            {
                DoubleLinkedList<T>.Node temp;
                var current = list.head;
                while(!(current is null))
                {
                    current.List = result;
                    temp = current.Next;
                    current.Next = current.Previous;
                    current.Previous = temp;

                    current = temp;
                }
                temp = list.head;
                list.head = list.tail;
                list.tail = temp;
            }
        }
    }
}
