using System;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NetFabric.DoublyLinkedList.Tests")]

namespace NetFabric
{
    public static class DoublyLinkedList
    {
        public static DoublyLinkedList<T> Append<T>(DoublyLinkedList<T> left, DoublyLinkedList<T> right) 
        {
            if (left is null)
                throw new ArgumentNullException(nameof(left));

            if (right is null)
                throw new ArgumentNullException(nameof(right));

            var result = new DoublyLinkedList<T>();
            result.AddLast(left);
            result.AddLast(right);
            return result;
        }
    }
}
