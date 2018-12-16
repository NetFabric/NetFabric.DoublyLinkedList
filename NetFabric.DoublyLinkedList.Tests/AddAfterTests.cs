using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
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
            action.Should()
                .ThrowExactly<ArgumentNullException>()
                .And
                .ParamName.Should()
                .Be("node");
        }

        [Fact]
        void InvalidNode()
        {
            // Arrange
            var list = new DoublyLinkedList<int>();
            var anotherList = new DoublyLinkedList<int>(new int[] { 1 });
            var node = anotherList.Find(1);

            // Act
            Action action = () => list.AddAfter(node, 1);

            // Assert
            action.Should().ThrowExactly<InvalidOperationException>();
        }

        public static TheoryData<IReadOnlyList<int>, int, int, IReadOnlyList<int>> ItemData =>
            new TheoryData<IReadOnlyList<int>, int, int, IReadOnlyList<int>>
            {
                { new int[] { 1 },      1, 2, new int[] { 1, 2 } },
                { new int[] { 1, 3 },   1, 2, new int[] { 1, 2, 3 } },
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
            list.Count.Should().Be(expected.Count);
            list.Version.Should().NotBe(version);
            list.EnumerateForward().Should().Equal(expected);
            list.EnumerateReversed().Should().Equal(expected.Reverse());
        }
    }
}
