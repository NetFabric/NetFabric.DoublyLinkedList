using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NetFabric.DoublyLinkedList.Tests")]

namespace NetFabric
{
    public static class DoublyLinkedList
    {
        [Pure]
        public static DoublyLinkedList<T> Append<T>(DoublyLinkedList<T> left, DoublyLinkedList<T> right) 
        {
            if (left is null) Throw.ArgumentNullException(nameof(left));
            if (right is null) Throw.ArgumentNullException(nameof(right));

            var result = new DoublyLinkedList<T>();
            result.AddLast(left);
            result.AddLast(right);
            return result;
        }
    }
}
