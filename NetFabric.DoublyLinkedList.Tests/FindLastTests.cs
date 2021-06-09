using NetFabric.Assertive;
using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Tests
{
    public class FindLastTests
    {
        public static TheoryData<IReadOnlyList<int?>, int?, bool> Data =>
            new()
            {
                { new int?[] { },                   null,   false },
                { new int?[] { },                   1,      false },
                { new int?[] { null },              null,   true },
                { new int?[] { null },              1,      false },
                { new int?[] { 1 },                 null,   false },
                { new int?[] { 1 },                 2,      false },
                { new int?[] { 1 },                 1,      true },
                { new int?[] { 1, 2, null, 4, 5 },  null,   true},
                { new int?[] { 1, 2, null, 4, 5 },  1,      true},
                { new int?[] { 1, 2, null, 4, 5 },  4,      true},
                { new int?[] { 1, 2, null, 4, 5 },  5,      true},
                { new int?[] { 1, 2, null, 4, 5 },  6,      false},
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void FindLast(IReadOnlyList<int?> collection, int? value, bool shouldFind)
        {
            // Arrange
            var list = new DoublyLinkedList<int?>(collection);

            // Act
            var result = list.FindLast(value);

            // Assert
            if (shouldFind)
            {
                result.Must()
                    .BeNotNull();
                result.Value.Must()
                    .BeEqualTo(value);
            }
            else
            {
                result.Must()
                    .BeNull();
            }
        }

        public static TheoryData<IReadOnlyList<int?>, int?> TailData =>
            new()
            {
                { new int?[] { null, null, null },  null },
                { new int?[] { 1, 1, 1 },           1 },
            };

        [Theory]
        [MemberData(nameof(TailData))]
        public void FindLastTail(IReadOnlyList<int?> collection, int? value)
        {
            // Arrange
            var list = new DoublyLinkedList<int?>(collection);

            // Act
            var result = list.FindLast(value);

            // Assert
            result.Must()
                .BeSameAs(list.Last);
        }
    }
}
