using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NetFabric.Tests
{
    public class ReverseTests
    {
        public static TheoryData<IReadOnlyList<int>, IReadOnlyList<int>> Data =>
            new TheoryData<IReadOnlyList<int>, IReadOnlyList<int>>
            {
                { new int[] { },                new int[] { } },
                { new int[] { 1 },              new int[] { 1 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { 5, 4, 3, 2, 1 } },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Reverse(IReadOnlyList<int> collection, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);

            // Act
            var result = list.Reverse();

            // Assert
            result.Version.Should().Be(0);
            result.Count.Should().Be(list.Count);
            result.EnumerateForward().Should().NotBeSameAs(list.EnumerateForward());
            result.EnumerateForward().Should().Equal(expected);
            result.EnumerateReversed().Should().Equal(collection);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void ReverseInPlace(IReadOnlyList<int> collection, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.ReverseInPlace();

            // Assert
            if(list.Count < 2)
                list.Version.Should().Be(version);
            else
                list.Version.Should().NotBe(version);
            list.Count.Should().Be(list.Count);
            list.EnumerateForward().Should().Equal(expected);
            list.EnumerateReversed().Should().Equal(collection);
        }
    }
}
