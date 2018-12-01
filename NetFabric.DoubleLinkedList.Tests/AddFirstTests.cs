using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NetFabric.Tests
{
    public class AddFirstTests
    {
        public static TheoryData<IEnumerable<int>, int, IReadOnlyCollection<int>> ItemData =>
            new TheoryData<IEnumerable<int>, int, IReadOnlyCollection<int>>
            {
                { new int[] { },            1, new int[] { 1 } },
                { new int[] { 2 },          1, new int[] { 1, 2 } },
                { new int[] { 2, 3, 4, 5 }, 1, new int[] { 1, 2, 3, 4, 5 } },
            };

        public static TheoryData<IEnumerable<int>, IEnumerable<int>, IReadOnlyCollection<int>> CollectionData =>
            new TheoryData<IEnumerable<int>, IEnumerable<int>, IReadOnlyCollection<int>>
            {
                { new int[] { },                new int[] { },                  new int[] { } },
                { new int[] { },                new int[] { 1 },                new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 },    new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1 },              new int[] { },                  new int[] { 1 } },
                { new int[] { 2 },              new int[] { 1 },                new int[] { 1, 2 } },
                { new int[] { 5 },              new int[] { 1, 2, 3, 4 },       new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 2, 3, 4, 5 },     new int[] { 1 },                new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 3, 4, 5 },        new int[] { 1, 2 },             new int[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(ItemData))]
        void AddItem(IEnumerable<int> collection, int item, IReadOnlyCollection<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.AddFirst(item);

            // Assert
            list.Count.Should().Be(expected.Count);
            list.Version.Should().NotBe(version);
            list.EnumerateForward().Should().Equal(expected);
            list.EnumerateReversed().Should().Equal(expected.Reverse());
        }

        [Theory]
        [MemberData(nameof(CollectionData))]
        void AddCollection(IEnumerable<int> collection, IEnumerable<int> items, IReadOnlyCollection<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.AddFirst(items);

            // Assert
            list.Count.Should().Be(expected.Count);
            list.Version.Should().NotBe(version);
            list.EnumerateForward().Should().Equal(expected);
            list.EnumerateReversed().Should().Equal(expected.Reverse());
        }
    }
}
