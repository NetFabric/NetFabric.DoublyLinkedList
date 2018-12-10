using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NetFabric.Tests
{
    [ExcludeFromCodeCoverage]
    public class AppendTests
    {
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
            var leftList = new DoubleLinkedList<int>(left);
            var leftVersion = leftList.Version;
            var rightList = new DoubleLinkedList<int>(right);
            var rightVersion = rightList.Version;

            // Act
            var result = DoubleLinkedList.Append(leftList, rightList);

            // Assert
            leftList.Version.Should().Be(leftVersion);
            leftList.EnumerateForward().Should().Equal(left);

            rightList.Version.Should().Be(rightVersion);
            rightList.EnumerateForward().Should().Equal(right);

            result.Count.Should().Be(leftList.Count + rightList.Count);
            result.EnumerateForward().Should().Equal(expected);
            result.EnumerateReversed().Should().Equal(expected.Reverse());        
        }
    }
}