using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using Xunit;

namespace NetFabric.Tests
{
    public class AppendTests
    {
        [Fact]
        void AppendNullLeft()
        {
            // Arrange

            // Act
            Action action = () => DoublyLinkedList.Append(null, new DoublyLinkedList<int>());

            // Assert
            action.Must()
                .Throw<ArgumentNullException>()
                .EvaluateTrue(exception =>
                    exception.ParamName == "left");
        }

        [Fact]
        void AppendNullRight()
        {
            // Arrange

            // Act
            Action action = () => DoublyLinkedList.Append(new DoublyLinkedList<int>(), null);

            // Assert
            action.Must()
                .Throw<ArgumentNullException>()
                .EvaluateTrue(exception =>
                    exception.ParamName == "right");
        }

        public static TheoryData<IReadOnlyList<int>, IReadOnlyList<int>, IReadOnlyList<int>> AppendData =>
            new()
            {
                { Array.Empty<int>(),       Array.Empty<int>(),         Array.Empty<int>() },
                { Array.Empty<int>(),       new[] { 1 } ,               new[] { 1 } },
                { new[] { 1 },              Array.Empty<int>(),         new[] { 1 } },
                { Array.Empty<int>(),       new[] { 1, 2, 3, 4, 5 } ,   new[] { 1, 2, 3, 4, 5 } },
                { new[] { 1, 2, 3, 4, 5 },  Array.Empty<int>(),         new[] { 1, 2, 3, 4, 5 } },
                { new[] { 1 },              new[] { 2, 3, 4, 5 },       new[] { 1, 2, 3, 4, 5 } },
                { new[] { 1, 2, 3, 4 },     new[] { 5 } ,               new[] { 1, 2, 3, 4, 5 } },
                { new[] { 1, 2, 3 },        new[] { 4, 5, 6 },          new[] { 1, 2, 3, 4, 5, 6 } },
            };

        [Theory]
        [MemberData(nameof(AppendData))]
        void Append(IReadOnlyList<int> left, IReadOnlyList<int> right, IReadOnlyCollection<int> expected)
        {
            // Arrange
            var leftList = new DoublyLinkedList<int>(left);
            var leftVersion = leftList.Version;
            var rightList = new DoublyLinkedList<int>(right);
            var rightVersion = rightList.Version;

            // Act
            var result = DoublyLinkedList.Append(leftList, rightList);

            // Assert
            leftList.Version.Must()
                .BeEqualTo(leftVersion);
            leftList.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(left);

            rightList.Version.Must()
                .BeEqualTo(rightVersion);
            rightList.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(right);

            result.Forward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected);
            result.Backward.Must()
                .BeEnumerableOf<int>()
                .BeEqualTo(expected.Reverse());        
        }
    }
}