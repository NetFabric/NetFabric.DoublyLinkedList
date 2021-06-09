using NetFabric.Assertive;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NetFabric.Tests
{
    public class CloneTests
    {
        public static TheoryData<IReadOnlyList<int>, IReadOnlyList<int>> Data =>
            new()
            {
                { Array.Empty<int>(),       Array.Empty<int>() },
                { new[] { 1 },              new[] { 1 } },
                { new[] { 1, 2, 3, 4, 5 },  new[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Clone(IReadOnlyList<int> collection, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);

            // Act
            var result = list.Clone();

            // Assert
            result.Version.Must()
                .BeEqualTo(0);
            result.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            result.Backward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(collection.Reverse());
        }
    }
}
