using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NetFabric.Tests
{
    [ExcludeFromCodeCoverage]
    public class CloneTests
    {
        public static TheoryData<IReadOnlyList<int>, IReadOnlyList<int>> Data =>
            new TheoryData<IReadOnlyList<int>, IReadOnlyList<int>>
            {
                { new int[] { },                new int[] { } },
                { new int[] { 1 },              new int[] { 1 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { 1, 2, 3, 4, 5 } },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Clone(IReadOnlyList<int> collection, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);

            // Act
            var result = list.Clone();

            // Assert
            result.Version.Should().Be(0);
            result.Count.Should().Be(list.Count);
            result.EnumerateForward().Should().NotBeSameAs(list.EnumerateForward());
            result.EnumerateForward().Should().Equal(expected);
            result.EnumerateForward().Should().NotBeSameAs(list.EnumerateForward());
            result.EnumerateReversed().Should().Equal(collection.Reverse());
        }
    }
}
