using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NetFabric.Tests
{
    public class RemoveTests
    {
        public static TheoryData<IEnumerable<int>, int, bool, IReadOnlyCollection<int>> RemoveItemData =>
            new TheoryData<IEnumerable<int>, int, bool, IReadOnlyCollection<int>>
            {
                { new int[] { },                1, false,   new int[] { } },
                { new int[] { 1 },              2, false,   new int[] { 1 } },
                { new int[] { 1 },              1, true,    new int[] { } },
                { new int[] { 1, 2, 3, 4, 5 },  6, false,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  1, true,    new int[] { 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  3, true,    new int[] { 1, 2, 4, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  5, true,    new int[] { 1, 2, 3, 4 } },
            };

        public static TheoryData<IEnumerable<int>, int, bool, IReadOnlyCollection<int>> RemoveFirstItemData =>
            new TheoryData<IEnumerable<int>, int, bool, IReadOnlyCollection<int>>
            {
                { new int[] { 1, 2, 3, 4, 5, 1 },  1, true, new int[] { 2, 3, 4, 5, 1 } },
            };

        public static TheoryData<IEnumerable<int>, int, bool, IReadOnlyCollection<int>> RemoveLastItemData =>
            new TheoryData<IEnumerable<int>, int, bool, IReadOnlyCollection<int>>
            {
                { new int[] { 1, 2, 3, 4, 5, 1 },  1, true, new int[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(RemoveItemData))]
        [MemberData(nameof(RemoveFirstItemData))]
        void RemoveItem(IEnumerable<int> collection, int item, bool expected, IReadOnlyCollection<int> expectedCollection)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            var result = list.Remove(item);

            // Assert
            result.Should().Be(expected);
            list.Count.Should().Be(expectedCollection.Count);
            if (expected)
                list.Version.Should().NotBe(version);
            else
                list.Version.Should().Be(version);
            list.EnumerateForward().Should().Equal(expectedCollection);
            list.EnumerateReversed().Should().Equal(expectedCollection.Reverse());
        }

        [Theory]
        [MemberData(nameof(RemoveItemData))]
        [MemberData(nameof(RemoveLastItemData))]
        void RemoveLastItem(IEnumerable<int> collection, int item, bool expected, IReadOnlyCollection<int> expectedCollection)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            var result = list.RemoveLast(item);

            // Assert
            result.Should().Be(expected);
            list.Count.Should().Be(expectedCollection.Count);
            if (expected)
                list.Version.Should().NotBe(version);
            else
                list.Version.Should().Be(version);
            list.EnumerateForward().Should().Equal(expectedCollection);
            list.EnumerateReversed().Should().Equal(expectedCollection.Reverse());
        }

        public static TheoryData<IEnumerable<int>, IReadOnlyCollection<int>> RemoveFirstData =>
            new TheoryData<IEnumerable<int>, IReadOnlyCollection<int>>
            {
                { new int[] { 1 },              new int[] { } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { 2, 3, 4, 5 } },
            };

        public static TheoryData<IEnumerable<int>, IReadOnlyCollection<int>> RemoveLastData =>
            new TheoryData<IEnumerable<int>, IReadOnlyCollection<int>>
            {
                { new int[] { 1 },              new int[] { } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { 1, 2, 3, 4 } },
            };

        [Theory]
        [MemberData(nameof(RemoveFirstData))]
        void RemoveFirst(IEnumerable<int> collection, IReadOnlyCollection<int> expectedCollection)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.RemoveFirst();

            // Assert
            list.Count.Should().Be(expectedCollection.Count);
            list.Version.Should().NotBe(version);
            list.EnumerateForward().Should().Equal(expectedCollection);
            list.EnumerateReversed().Should().Equal(expectedCollection.Reverse());
        }

        [Theory]
        [MemberData(nameof(RemoveLastData))]
        void RemoveLast(IEnumerable<int> collection, IReadOnlyCollection<int> expectedCollection)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.RemoveLast();

            // Assert
            list.Count.Should().Be(expectedCollection.Count);
            list.Version.Should().NotBe(version);
            list.EnumerateForward().Should().Equal(expectedCollection);
            list.EnumerateReversed().Should().Equal(expectedCollection.Reverse());
        }

        [Theory]
        [MemberData(nameof(RemoveFirstData))]
        void RemoveFirst_WithEmpty_ShouldThrow(IEnumerable<int> collection, IReadOnlyCollection<int> expectedCollection)
        {
            // Arrange
            var list = new DoubleLinkedList<int>();

            // Act
            Action action = () => list.RemoveFirst();

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [MemberData(nameof(RemoveFirstData))]
        void RemoveLast_WithEmpty_ShouldThrow(IEnumerable<int> collection, IReadOnlyCollection<int> expectedCollection)
        {
            // Arrange
            var list = new DoubleLinkedList<int>();

            // Act
            Action action = () => list.RemoveLast();

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

    }
}
