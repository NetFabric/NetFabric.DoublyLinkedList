using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NetFabric.DoubleLinkedList.Tests
{
    public class ReverseEnumerationTests
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
        public void Enumeration(IEnumerable<int> collection, IEnumerable<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);

            // Act
            var enumeration = list.EnumerateReversed();

            // Assert
            enumeration.Should().Equal(expected);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Reset(IEnumerable<int> collection, IEnumerable<int> expected)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            using (var enumerator = list.EnumerateReversed().GetEnumerator())
            using (var expectedEnumerator = expected.GetEnumerator())
            {
                var moveNext = enumerator.MoveNext();
                var expectedMoveNext = expectedEnumerator.MoveNext();
                var current = -1;
                var expectedCurrent = -1;
                if (moveNext)
                {
                    current = enumerator.Current;
                    expectedCurrent = expectedEnumerator.Current;
                }

                // Act
                enumerator.Reset();

                // Assert
                enumerator.MoveNext().Should().Be(expectedMoveNext);
                if (moveNext)
                {
                    enumerator.Current.Should().Be(expectedCurrent);
                }
            }
        }
    }
}
