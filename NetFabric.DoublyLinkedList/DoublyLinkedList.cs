using System;
using System.Diagnostics.Contracts;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NetFabric.DoublyLinkedList.Tests")]

namespace NetFabric
{
    public static class DoublyLinkedList
    {
        [Pure]
        public static DoublyLinkedList<T> Append<T>(DoublyLinkedList<T> left, DoublyLinkedList<T> right) 
        {
            if (left is null)
                ThrowLeftNull();

            if (right is null)
                ThrowRightNull();

            var result = new DoublyLinkedList<T>();
            result.AddLast(left);
            result.AddLast(right);
            return result;

            void ThrowLeftNull() => throw new ArgumentNullException(nameof(left));
            void ThrowRightNull() => throw new ArgumentNullException(nameof(right));
        }
    }
}
