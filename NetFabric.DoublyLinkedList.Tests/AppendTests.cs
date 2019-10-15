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
                .EvaluatesTrue(exception =>
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
                .EvaluatesTrue(exception =>
                    exception.ParamName == "right");
        }

        public static TheoryData<IReadOnlyList<int>, IReadOnlyList<int>, IReadOnlyList<int>> AppendData =>
            new TheoryData<IReadOnlyList<int>, IReadOnlyList<int>, IReadOnlyList<int>>
            {
                { new int[] { },                new int[] { },                  new int[] { } },
                { new int[] { },                new int[] { 1 } ,               new int[] { 1 } },
                { new int[] { 1 },              new int[] { },                  new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 } ,   new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1 },              new int[] { 2, 3, 4, 5 },       new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4 },     new int[] { 5 } ,               new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3 },        new int[] { 4, 5, 6 },          new int[] { 1, 2, 3, 4, 5, 6 } },
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
            leftList.EnumerateForward().Must()
                .BeEnumerable<int>()
                .BeEqualTo(left);

            rightList.Version.Must()
                .BeEqualTo(rightVersion);
            rightList.EnumerateForward().Must()
                .BeEnumerable<int>()
                .BeEqualTo(right);

            result.EnumerateForward().Must()
                .BeEnumerable<int>()
                .BeEqualTo(expected);
            result.EnumerateReversed().Must()
                .BeEnumerable<int>()
                .BeEqualTo(expected.Reverse());        
        }
    }
}