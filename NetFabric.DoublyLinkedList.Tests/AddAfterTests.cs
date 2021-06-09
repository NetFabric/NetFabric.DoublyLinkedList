using NetFabric.Assertive;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NetFabric.Tests
{
    public class AddAfterTests
    {
        [Fact]
        void NullNode()
        {
            // Arrange
            var list = new DoublyLinkedList<int>();

            // Act
            Action action = () => list.AddAfter(null, 1);

            // Assert
            action.Must()
                .Throw<ArgumentNullException>()
                .EvaluateTrue(exception =>
                    exception.ParamName == "node");
        }

        [Fact]
        void InvalidNode()
        {
            // Arrange
            var list = new DoublyLinkedList<int>();
            var anotherList = new DoublyLinkedList<int>(new[] { 1 });
            var node = anotherList.Find(1);

            // Act
            Action action = () => list.AddAfter(node, 1);

            // Assert
            action.Must()
                .Throw<InvalidOperationException>();
        }

        public static TheoryData<IReadOnlyList<int>, int, int, IReadOnlyList<int>> ItemData =>
            new()
            {
                { new[] { 1 },      1, 2, new[] { 1, 2 } },
                { new[] { 1, 3 },   1, 2, new[] { 1, 2, 3 } },
            };

        [Theory]
        [MemberData(nameof(ItemData))]
        void AddItem(IReadOnlyList<int> collection, int after, int item, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);
            var version = list.Version;
            var node = list.Find(after);

            // Act
            list.AddAfter(node, item);

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
    }
}
