using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace NetFabric.Tests
{
    [ExcludeFromCodeCoverage]
    public class ForwardEnumerationTests
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
        public void Enumeration(IReadOnlyList<int> collection)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);

            // Act
            var enumeration = list.EnumerateForward();

            // Assert
            enumeration.Should().Equal(collection);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Reset(IReadOnlyList<int> collection)
        {
            // Arrange
            var list = new DoubleLinkedList<int>(collection);
            using (var enumerator = list.EnumerateForward().GetEnumerator())
            {
                var moveNext = enumerator.MoveNext();
                int first = -1;
                if (moveNext)
                    first = enumerator.Current;

                // Act
                enumerator.Reset();

                // Assert
                enumerator.MoveNext().Should().Be(moveNext);
                if (moveNext)
                    enumerator.Current.Should().Be(first);
            }
        }
    }
}
