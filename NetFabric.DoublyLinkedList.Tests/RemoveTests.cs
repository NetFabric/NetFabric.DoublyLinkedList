using NetFabric.Assertive;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NetFabric.Tests
{
    public class RemoveTests
    {
        public static TheoryData<IReadOnlyList<int>, int, bool, IReadOnlyCollection<int>> RemoveItemData =>
            new()
            {
                { Array.Empty<int>(),                1, false,   Array.Empty<int>() },
                { new[] { 1 },              2, false,   new[] { 1 } },
                { new[] { 1 },              1, true,    Array.Empty<int>() },
                { new[] { 1, 2, 3, 4, 5 },  6, false,   new[] { 1, 2, 3, 4, 5 } },
                { new[] { 1, 2, 3, 4, 5 },  1, true,    new[] { 2, 3, 4, 5 } },
                { new[] { 1, 2, 3, 4, 5 },  3, true,    new[] { 1, 2, 4, 5 } },
                { new[] { 1, 2, 3, 4, 5 },  5, true,    new[] { 1, 2, 3, 4 } },
            };

        public static TheoryData<IReadOnlyList<int>, int, bool, IReadOnlyCollection<int>> RemoveFirstItemData =>
            new()
            {
                { new[] { 1, 2, 3, 4, 5, 1 },  1, true, new[] { 2, 3, 4, 5, 1 } },
            };

        public static TheoryData<IReadOnlyList<int>, int, bool, IReadOnlyCollection<int>> RemoveLastItemData =>
            new()
            {
                { new[] { 1, 2, 3, 4, 5, 1 },  1, true, new[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(RemoveItemData))]
        [MemberData(nameof(RemoveFirstItemData))]
        void RemoveItem(IReadOnlyList<int> collection, int item, bool expected, IReadOnlyCollection<int> expectedCollection)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);
            var version = list.Version;

            // Act
            var result = list.Remove(item);

            // Assert
            result.Must().BeEqualTo(expected);
            if (expected)
                list.Version.Must().BeNotEqualTo(version);
            else
                list.Version.Must().BeEqualTo(version);
            list.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expectedCollection);
            list.Backward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expectedCollection.Reverse());
        }

        [Theory]
        [MemberData(nameof(RemoveItemData))]
        [MemberData(nameof(RemoveLastItemData))]
        void RemoveLastItem(IReadOnlyList<int> collection, int item, bool expected, IReadOnlyCollection<int> expectedCollection)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);
            var version = list.Version;

            // Act
            var result = list.RemoveLast(item);

            // Assert
            result.Must().
                BeEqualTo(expected);
            if (expected)
                list.Version.Must().BeNotEqualTo(version);
            else
                list.Version.Must().BeEqualTo(version);
            list.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expectedCollection);
            list.Backward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expectedCollection.Reverse());
        }

        public static TheoryData<IReadOnlyList<int>, IReadOnlyCollection<int>> RemoveFirstData =>
            new()
            {
                { new[] { 1 },              Array.Empty<int>() },
                { new[] { 1, 2, 3, 4, 5 },  new[] { 2, 3, 4, 5 } },
            };

        public static TheoryData<IReadOnlyList<int>, IReadOnlyCollection<int>> RemoveLastData =>
            new()
            {
                { new[] { 1 },              Array.Empty<int>() },
                { new[] { 1, 2, 3, 4, 5 },  new[] { 1, 2, 3, 4 } },
            };

        [Theory]
        [MemberData(nameof(RemoveFirstData))]
        void RemoveFirst(IReadOnlyList<int> collection, IReadOnlyCollection<int> expectedCollection)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.RemoveFirst();

            // Assert
            list.Version.Must()
                .BeNotEqualTo(version);
            list.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expectedCollection);
            list.Backward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expectedCollection.Reverse());
        }

        [Theory]
        [MemberData(nameof(RemoveLastData))]
        void RemoveLast(IReadOnlyList<int> collection, IReadOnlyCollection<int> expectedCollection)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.RemoveLast();

            // Assert
            list.Version.Must()
                .BeNotEqualTo(version);
            list.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expectedCollection);
            list.Backward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expectedCollection.Reverse());
        }

        [Fact]
        void RemoveFirst_WithEmpty_MustThrow()
        {
            // Arrange
            var list = new DoublyLinkedList<int>();

            // Act
            Action action = () => list.RemoveFirst();

            // Assert
            action.Must()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        void RemoveLast_WithEmpty_MustThrow()
        {
            // Arrange
            var list = new DoublyLinkedList<int>();

            // Act
            Action action = () => list.RemoveLast();

            // Assert
            action.Must()
                .Throw<InvalidOperationException>();
        }

    }
}
