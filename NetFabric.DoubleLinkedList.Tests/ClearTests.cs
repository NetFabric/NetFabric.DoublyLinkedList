using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NetFabric.Tests
{
    public class ClearTests
    {
        public static TheoryData<IReadOnlyList<int>> Data =>
            new TheoryData<IReadOnlyList<int>>
            {
                new int[] { },
                new int[] { 1 },
                new int[] { 1, 2, 3, 4, 5 },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Clear(IReadOnlyList<int> collection)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            var version = list.Version;

            // Act
            list.Clear();

            // Assert
            list.Count.Should().Be(0);
            list.Version.Should().NotBe(version);
            list.EnumerateForward().Should().BeEmpty();
            list.EnumerateReversed().Should().BeEmpty();
        }
    }
}
