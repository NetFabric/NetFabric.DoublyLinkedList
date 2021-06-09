using System;
using System.Collections.Generic;
using NetFabric.Assertive;
using Xunit;

namespace NetFabric.Tests
{
    public class ReverseEnumerationTests
    {
        public static TheoryData<IReadOnlyList<int>, IReadOnlyList<int>> Data =>
            new()
            {
                { Array.Empty<int>(),       Array.Empty<int>() },
                { new[] { 1 },              new[] { 1 } },
                { new[] { 1, 2, 3, 4, 5 },  new[] { 5, 4, 3, 2, 1 } },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Enumeration(IReadOnlyList<int> collection, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);

            // Act
            var enumeration = list.Backward;

            // Assert
            enumeration.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
        }
    }
}
