using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NetFabric.DoubleLinkedList.Tests
{
    public class ReverseTests
    {
        public static TheoryData<IEnumerable<int>, IEnumerable<int>> Data =>
            new TheoryData<IEnumerable<int>, IEnumerable<int>>
            {
                { new int[] { },                new int[] { } },
                { new int[] { 1 },              new int[] { 1 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { 5, 4, 3, 2, 1 } },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Reverse(IEnumerable<int> collection, IEnumerable<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.Reverse();

            // Assert
            list.Version.Should().NotBe(version);
            list.EnumerateForward().Should().Equal(expected);
            list.EnumerateReversed().Should().Equal(expected.Reverse());
        }
    }
}
