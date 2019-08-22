using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NetFabric.Tests
{
    public class ReverseEnumerationTests
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
        public void Enumeration(IReadOnlyList<int> collection, IReadOnlyList<int> expected)
        {
            // Arrange
            var list = new DoublyLinkedList<int>(collection);

            // Act
            var enumeration = list.EnumerateReversed();

            // Assert
            enumeration.Should().Equal(expected);
        }
    }
}
