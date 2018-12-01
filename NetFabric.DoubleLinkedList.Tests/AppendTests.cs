using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NetFabric.Tests
{
    public class AppendTests
    {
        public static TheoryData<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>> AppendData =>
            new TheoryData<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>>
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
        void Append(IEnumerable<int> left, IEnumerable<int> right, IReadOnlyCollection<int> expected)
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

        public static TheoryData<IEnumerable<int>, IEnumerable<int>, bool, bool, IEnumerable<int>> AppendInPlaceData =>
            new TheoryData<IEnumerable<int>, IEnumerable<int>, bool, bool, IEnumerable<int>>
            {
                { new int[] { },                new int[] { },                  false,  false, new int[] { } },
                { new int[] { },                new int[] { 1 } ,               false,  false, new int[] { 1 } },
                { new int[] { 1 },              new int[] { },                  false,  false, new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 } ,   false,  false, new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  false,  false, new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1 },              new int[] { 2, 3, 4, 5 },       false,  false, new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4 },     new int[] { 5 } ,               false,  false, new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3 },        new int[] { 4, 5, 6 },          false,  false, new int[] { 1, 2, 3, 4, 5, 6 } },
                { new int[] { },                new int[] { },                  true,   false, new int[] { } },
                { new int[] { },                new int[] { 1 } ,               true,   false, new int[] { 1 } },
                { new int[] { 1 },              new int[] { },                  true,   false, new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 } ,   true,   false, new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  true,   false, new int[] { 5, 4, 3, 2, 1 } },
                { new int[] { 1 },              new int[] { 2, 3, 4, 5 },       true,   false, new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3, 4 },     new int[] { 5 } ,               true,   false, new int[] { 4, 3, 2, 1, 5 } },
                { new int[] { 1, 2, 3 },        new int[] { 4, 5, 6 },          true,   false, new int[] { 3, 2, 1, 4, 5, 6 } },
                { new int[] { },                new int[] { },                  false,  true,  new int[] { } },
                { new int[] { },                new int[] { 1 } ,               false,  true,  new int[] { 1 } },
                { new int[] { 1 },              new int[] { },                  false,  true,  new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 } ,   false,  true,  new int[] { 5, 4, 3, 2, 1 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  false,  true,  new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1 },              new int[] { 2, 3, 4, 5 },       false,  true,  new int[] { 1, 5, 4, 3, 2 } },
                { new int[] { 1, 2, 3, 4 },     new int[] { 5 } ,               false,  true,  new int[] { 1, 2, 3, 4, 5 } },
                { new int[] { 1, 2, 3 },        new int[] { 4, 5, 6 },          false,  true,  new int[] { 1, 2, 3, 6, 5, 4 } },
                { new int[] { },                new int[] { },                  true,   true,  new int[] { } },
                { new int[] { },                new int[] { 1 } ,               true,   true,  new int[] { 1 } },
                { new int[] { 1 },              new int[] { },                  true,   true,  new int[] { 1 } },
                { new int[] { },                new int[] { 1, 2, 3, 4, 5 } ,   true,   true,  new int[] { 5, 4, 3, 2, 1 } },
                { new int[] { 1, 2, 3, 4, 5 },  new int[] { },                  true,   true,  new int[] { 5, 4, 3, 2, 1 } },
                { new int[] { 1 },              new int[] { 2, 3, 4, 5 },       true,   true,  new int[] { 1, 5, 4, 3, 2 } },
                { new int[] { 1, 2, 3, 4 },     new int[] { 5 } ,               true,   true,  new int[] { 4, 3, 2, 1, 5 } },
                { new int[] { 1, 2, 3 },        new int[] { 4, 5, 6 },          true,   true,  new int[] {3, 2, 1, 6, 5, 4 } },
            };

        [Theory]
        [MemberData(nameof(AppendInPlaceData))]
        void AppendInPlace(IEnumerable<int> left, IEnumerable<int> right, bool reverseLeft, bool reverseRight, IReadOnlyCollection<int> expected)
        {
            // Arrange
            var leftList = new DoubleLinkedList<int>(left);
            var leftVersion = leftList.Version;
            var rightList = new DoubleLinkedList<int>(right);
            var rightVersion = rightList.Version;

            // Act
            var result = DoubleLinkedList.AppendInPlace(leftList, rightList, reverseLeft, reverseRight);

            // Assert
            leftList.Count.Should().Be(0);
            leftList.Version.Should().NotBe(leftVersion);
            leftList.EnumerateForward().Should().Equal(Enumerable.Empty<int>());
            leftList.EnumerateReversed().Should().Equal(Enumerable.Empty<int>());

            rightList.Count.Should().Be(0);
            rightList.Version.Should().NotBe(rightVersion);
            rightList.EnumerateForward().Should().Equal(Enumerable.Empty<int>());
            rightList.EnumerateReversed().Should().Equal(Enumerable.Empty<int>());        

            result.Count.Should().Be(leftList.Count + rightList.Count);
            result.EnumerateForward().Should().Equal(expected);
            result.EnumerateReversed().Should().Equal(expected.Reverse());        
        }
    }
}