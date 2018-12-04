using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NetFabric.Tests
{
    public class CloneTests
    {
        public static TheoryData<IEnumerable<int>, IEnumerable<int>> Data =>
            new TheoryData<IEnumerable<int>, IEnumerable<int>>
            {
                { new int[] { },                new int[] { } },
                { new int[] { 1 },              new int[] { 1 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Clone(IEnumerable<int> collection, IEnumerable<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);

            // Act
            var result = list.Clone();

            // Assert
            result.Version.Should().Be(0);
            result.Count.Should().Be(list.Count);
            result.EnumerateForward().Should().Equal(expected);
            result.EnumerateReversed().Should().Equal(collection.Reverse());
        }
    }
}
