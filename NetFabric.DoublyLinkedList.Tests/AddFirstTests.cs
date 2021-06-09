using NetFabric.Assertive;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NetFabric.Tests
{
    public class AddFirstTests
    {
        [Fact]
        void AddNullEnumerable()
        {
            // Arrange
            var list = new DoublyLinkedList<int>();

            // Act
            Action action = () => list.AddFirst((IEnumerable<int>)null);

            // Assert
            action.Must()
                .Throw<ArgumentNullException>()
                .EvaluateTrue(exception =>
                    exception.ParamName == "collection");
        }

        [Fact]
        void AddNullCollection()
        {
            // Arrange
            var list = new DoublyLinkedList<int>();

            // Act
            Action action = () => list.AddFirst((IReadOnlyList<int>)null);

            // Assert
            action.Must()
                .Throw<ArgumentNullException>()
                .EvaluateTrue(exception =>
                    exception.ParamName == "collection");
        }

        [Fact]
        void AddNullList()
        {
            // Arrange
            var list = new DoublyLinkedList<int>();

            // Act
            Action action = () => list.AddFirst((DoublyLinkedList<int>)null);

            // Assert
            action.Must()
                .Throw<ArgumentNullException>()
                .EvaluateTrue(exception =>
                    exception.ParamName == "list");
        }

        public static TheoryData<IReadOnlyList<int>, int, IReadOnlyList<int>> ItemData =>
            new()
            {
                { Array.Empty<int>(),   1, new[] { 1 } },
                { new[] { 2 },          1, new[] { 1, 2 } },
                { new[] { 2, 3, 4, 5 }, 1, new[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(ItemData))]
        void AddItem(IReadOnlyList<int> collection, int item, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.AddFirst(item);

            // Assert
            list.Version.Must()
                .BeNotEqualTo(version);
            list.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            list.Backward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected.Reverse());
        }

        public static TheoryData<IReadOnlyList<int>, IReadOnlyList<int>, bool, IReadOnlyList<int>> CollectionData =>
            new()
            {
                { Array.Empty<int>(),       Array.Empty<int>(),         false,  Array.Empty<int>() },
                { Array.Empty<int>(),       new[] { 1 },                true,   new[] { 1 } },
                { Array.Empty<int>(),       new[] { 1, 2, 3, 4, 5 },    true,   new[] { 1, 2, 3, 4, 5 } },
                { new[] { 1 },              Array.Empty<int>(),         false,  new[] { 1 } },
                { new[] { 2 },              new[] { 1 },                true,   new[] { 1, 2 } },
                { new[] { 5 },              new[] { 1, 2, 3, 4 },       true,   new[] { 1, 2, 3, 4, 5 } },
                { new[] { 1, 2, 3, 4, 5 },  Array.Empty<int>(),         false,  new[] { 1, 2, 3, 4, 5 } },
                { new[] { 2, 3, 4, 5 },     new[] { 1 },                true,   new[] { 1, 2, 3, 4, 5 } },
                { new[] { 3, 4, 5 },        new[] { 1, 2 },             true,   new[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(CollectionData))]
        void AddEnumerable(IReadOnlyList<int> collection, IEnumerable<int> items, bool isMutated, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.AddFirst(items);

            // Assert
            if (isMutated)
                list.Version.Must().BeNotEqualTo(version);
            else
                list.Version.Must().BeEqualTo(version);
            list.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            list.Backward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected.Reverse());
        }

        [Theory]
        [MemberData(nameof(CollectionData))]
        void AddCollection(IReadOnlyList<int> collection, IReadOnlyList<int> items, bool isMutated, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.AddFirst(items);

            // Assert
            if (isMutated)
                list.Version.Must().BeNotEqualTo(version);
            else
                list.Version.Must().BeEqualTo(version);
            list.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            list.Backward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected.Reverse());
        }

        public static TheoryData<IReadOnlyList<int>, IReadOnlyList<int>, bool, bool, IReadOnlyList<int>> ListData =>
            new()
            {
                { Array.Empty<int>(),       Array.Empty<int>(),         false,  false,  Array.Empty<int>() },
                { Array.Empty<int>(),       new[] { 1 },                false,  true,   new[] { 1 } },
                { Array.Empty<int>(),       new[] { 1, 2, 3, 4, 5 },    false,  true,   new[] { 1, 2, 3, 4, 5 } },
                { new[] { 1 },              Array.Empty<int>(),         false,  false,  new[] { 1 } },
                { new[] { 2 },              new[] { 1 },                false,  true,   new[] { 1, 2 } },
                { new[] { 5 },              new[] { 1, 2, 3, 4 },       false,  true,   new[] { 1, 2, 3, 4, 5 } },
                { new[] { 1, 2, 3, 4, 5 },  Array.Empty<int>(),         false,  false,  new[] { 1, 2, 3, 4, 5 } },
                { new[] { 2, 3, 4, 5 },     new[] { 1 },                false,  true,   new[] { 1, 2, 3, 4, 5 } },
                { new[] { 3, 4, 5 },        new[] { 1, 2 },             false,  true,   new[] { 1, 2, 3, 4, 5 } },
                { Array.Empty<int>(),       Array.Empty<int>(),         true,   false,  Array.Empty<int>() },
                { Array.Empty<int>(),       new[] { 1 },                true,   true,   new[] { 1 } },
                { Array.Empty<int>(),       new[] { 1, 2, 3, 4, 5 },    true,   true,   new[] { 5, 4, 3, 2, 1 } },
                { new[] { 1 },              Array.Empty<int>(),         true,   false,  new[] { 1 } },
                { new[] { 2 },              new[] { 1 },                true,   true,   new[] { 1, 2 } },
                { new[] { 5 },              new[] { 1, 2, 3, 4 },       true,   true,   new[] { 4, 3, 2, 1, 5 } },
                { new[] { 1, 2, 3, 4, 5 },  Array.Empty<int>(),         true,   false,  new[] { 1, 2, 3, 4, 5 } },
                { new[] { 2, 3, 4, 5 },     new[] { 1 },                true,   true,   new[] { 1, 2, 3, 4, 5 } },
                { new[] { 3, 4, 5 },        new[] { 1, 2 },             true,   true,   new[] { 2, 1, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(ListData))]
        void AddList(IReadOnlyList<int> collection, IReadOnlyList<int> items, bool reversed, bool isMutated, IReadOnlyList<int> expected)
        {
            // Arrange
            var left = new DoublyLinkedList<int>(collection);
            var version = left.Version;
            var right = new DoublyLinkedList<int>(items);

            // Act
            left.AddFirst(right, reversed);

            // Assert
            if (isMutated)
                left.Version.Must().BeNotEqualTo(version);
            else
                left.Version.Must().BeEqualTo(version);
            left.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            left.Backward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected.Reverse());
        }

        [Theory]
        [MemberData(nameof(ListData))]
        void AddListFrom(IReadOnlyList<int> collection, IReadOnlyList<int> items, bool reversed, bool isMutated, IReadOnlyList<int> expected)
        {
            // Arrange
            var left = new DoublyLinkedList<int>(collection);
            var version = left.Version;
            var right = new DoublyLinkedList<int>(items);

            // Act
            left.AddFirstFrom(right, reversed);

            // Assert
            if (isMutated)
                left.Version.Must().BeNotEqualTo(version);
            else
                left.Version.Must().BeEqualTo(version);
            left.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            left.Backward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected.Reverse());
            right.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEmpty();
        }
    }
}
