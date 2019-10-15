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
            new TheoryData<IReadOnlyList<int>, IReadOnlyList<int>>
            {
                { new int[] { },                new int[] { } },
                { new int[] { 1 },              new int[] { 1 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { 1, 2, 3, 4, 5 } },
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
            result.EnumerateForward().Must()
                .BeEnumerable<int>()
                .BeEqualTo(expected);
            result.EnumerateReversed().Must()
                .BeEnumerable<int>()
                .BeEqualTo(collection.Reverse());
        }
    }
}
