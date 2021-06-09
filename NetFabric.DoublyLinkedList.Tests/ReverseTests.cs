using NetFabric.Assertive;
using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Tests
{
    public class ReverseTests
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
        public void Reverse(IReadOnlyList<int> collection, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);

            // Act
            var result = list.Reverse();

            // Assert
            result.Version.Must()
                .BeEqualTo(0);
            result.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            result.Backward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(collection);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void ReverseInPlace(IReadOnlyList<int> collection, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.ReverseInPlace();

            // Assert
            if (list.Count < 2)
                list.Version.Must().BeEqualTo(version);
            else
                list.Version.Must().BeNotEqualTo(version);
            list.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            list.Backward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(collection);
        }
    }
}
