using Xunit;
using MovieAPI;

namespace MovieTests
{
    public class UtilityTest
    {

        [Theory]
        [InlineData(2.91,3.0)]
        [InlineData(3.249, 3.0)]
        [InlineData(3.25, 3.5)]
        [InlineData(3.6, 3.5)]
        [InlineData(3.75, 4.0)]
        [InlineData(1, 1)]
        [InlineData(3.5, 3.5)]
        [InlineData(1.25, 1.5)]
        [InlineData(1.75, 2)]

        public void RoundingShould_GiveTheCorrectValue(double input, double expected)
        {
            var calculated = Utilities.RoundToTheNearestHalf(input);
            Assert.Equal(expected, calculated);
        }
    }
}
