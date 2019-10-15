using System;
using System.Collections.Generic;
using NetFabric.Assertive;
using Xunit;

namespace NetFabric.Tests
{
    public class ClearTests
    {
        public static TheoryData<IReadOnlyList<int>> Data =>
            new TheoryData<IReadOnlyList<int>>
            {
                new int[] { },
                new int[] { 1 },
                new int[] { 1, 2, 3, 4, 5 },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Clear(IReadOnlyList<int> collection)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.Clear();

            // Assert
            list.Version.Must()
                .BeNotEqualTo(version);
            list.EnumerateForward().Must()
                .BeEnumerable<int>()
                .BeEmpty();
            list.EnumerateReversed().Must()
                .BeEnumerable<int>()
                .BeEmpty();
        }
    }
}
