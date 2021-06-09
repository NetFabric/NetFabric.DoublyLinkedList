using NetFabric.Assertive;
using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Tests
{
    public class ForwardEnumerationTests
    {
        public static TheoryData<IReadOnlyList<int>> Data =>
            new()
            {
                Array.Empty<int>(),
                new[] { 1 },
                new[] { 1, 2, 3, 4, 5 },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Enumeration(IReadOnlyList<int> collection)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);

            // Act
            var enumeration = list.Forward;

            // Assert
            enumeration.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(collection);
        }
    }
}
