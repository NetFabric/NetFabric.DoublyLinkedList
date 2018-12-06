using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NetFabric.Tests
{
    public class AddLastTests
    {
        public static TheoryData<IEnumerable<int>, int, IReadOnlyCollection<int>> ItemData =>
            new TheoryData<IEnumerable<int>, int, IReadOnlyCollection<int>>
            {
                { new int[] { },            1, new int[] { 1 } }, 
                { new int[] { 1 },          2, new int[] { 1, 2 } },
                { new int[] { 1, 2, 3, 4 }, 5, new int[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(ItemData))]
        void AddItem(IEnumerable<int> collection, int item, IReadOnlyCollection<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.AddLast(item);

            // Assert
            list.Count.Should().Be(expected.Count);
            list.Version.Should().NotBe(version);
            list.EnumerateForward().Should().Equal(expected);
            list.EnumerateReversed().Should().Equal(expected.Reverse());
        }

        public static TheoryData<IEnumerable<int>, IEnumerable<int>, bool, IReadOnlyCollection<int>> CollectionData =>
            new TheoryData<IEnumerable<int>, IEnumerable<int>, bool, IReadOnlyCollection<int>>
            {
                { new int[] { },                new int[] { },                  false,  new int[] { } },
                { new int[] { },                new int[] { 1 },                true,   new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 },    true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1 },              new int[] { },                  false,  new int[] { 1 } },
                { new int[] { 1 },              new int[] { 2 },                true,   new int[] { 1, 2 } },
                { new int[] { 1 },              new int[] { 2, 3, 4, 5 },       true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  false,  new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4 },     new int[] { 5 },                true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3 },        new int[] { 4, 5 },             true,   new int[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(CollectionData))]
        void AddCollection(IEnumerable<int> collection, IEnumerable<int> items, bool isMutated, IReadOnlyCollection<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.AddLast(items);

            // Assert
            list.Count.Should().Be(expected.Count);
            if (isMutated)
                list.Version.Should().NotBe(version);
            else
                list.Version.Should().Be(version);
            list.EnumerateForward().Should().Equal(expected);
            list.EnumerateReversed().Should().Equal(expected.Reverse());
        }

        public static TheoryData<IEnumerable<int>, IEnumerable<int>, bool, bool, IReadOnlyCollection<int>> ListData =>
            new TheoryData<IEnumerable<int>, IEnumerable<int>, bool, bool, IReadOnlyCollection<int>>
            {
                { new int[] { },                new int[] { },                  false,  false,  new int[] { } },
                { new int[] { },                new int[] { 1 },                false,  true,   new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 },    false,  true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1 },              new int[] { },                  false,  false,  new int[] { 1 } },
                { new int[] { 1 },              new int[] { 2 },                false,  true,   new int[] { 1, 2 } },
                { new int[] { 1 },              new int[] { 2, 3, 4, 5 },       false,  true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  false,  false,  new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4 },     new int[] { 5 },                false,  true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3 },        new int[] { 4, 5 },             false,  true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { },                new int[] { },                  true,   false,  new int[] { } },
                { new int[] { },                new int[] { 1 },                true,   true,   new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 },    true,   true,   new int[] { 5, 4, 3, 2, 1 } },
                { new int[] { 1 },              new int[] { },                  true,   false,  new int[] { 1 } },
                { new int[] { 1 },              new int[] { 2 },                true,   true,   new int[] { 1, 2 } },
                { new int[] { 1 },              new int[] { 2, 3, 4, 5 },       true,   true,   new int[] { 1, 5, 4, 3, 2 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  true,   false,  new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4 },     new int[] { 5 },                true,   true,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3 },        new int[] { 4, 5 },             true,   true,   new int[] { 1, 2, 3, 5, 4 } },
            };

        [Theory]
        [MemberData(nameof(ListData))]
        void AddList(IEnumerable<int> collection, IEnumerable<int> items, bool reverse, bool isMutated, IReadOnlyCollection<int> expected)
        {
            // Arrange
            var left = new DoubleLinkedList<int>(collection);
            var version = left.Version;
            var right = new DoubleLinkedList<int>(items);

            // Act
            left.AddLast(right, reverse);

            // Assert
            left.Count.Should().Be(expected.Count);
            if (isMutated)
                left.Version.Should().NotBe(version);
            else
                left.Version.Should().Be(version);
            left.EnumerateForward().Should().Equal(expected);
            left.EnumerateReversed().Should().Equal(expected.Reverse());
        }

        [Theory]
        [MemberData(nameof(ListData))]
        void AddListFrom(IEnumerable<int> collection, IEnumerable<int> items, bool reverse, bool isMutated, IReadOnlyCollection<int> expected)
        {
            // Arrange
            var left = new DoubleLinkedList<int>(collection);
            var version = left.Version;
            var right = new DoubleLinkedList<int>(items);

            // Act
            left.AddLastFrom(right, reverse);

            // Assert
            left.Count.Should().Be(expected.Count);
            if (isMutated)
                left.Version.Should().NotBe(version);
            else
                left.Version.Should().Be(version);
            left.EnumerateForward().Should().Equal(expected);
            left.EnumerateReversed().Should().Equal(expected.Reverse());
            right.Count.Should().Be(0);
        }
    }
}
