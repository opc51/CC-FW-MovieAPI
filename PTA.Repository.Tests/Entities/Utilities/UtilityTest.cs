using NUnit.Framework;

namespace Movie.Repository.Tests.Entities.Utility
{
    public class UtilityTest
    {
        [TestCase(2.91, 3.0)]
        [TestCase(3.249, 3.0)]
        [TestCase(3.25, 3.5)]
        [TestCase(3.6, 3.5)]
        [TestCase(3.75, 4.0)]
        [TestCase(1, 1)]
        [TestCase(3.5, 3.5)]
        [TestCase(1.24, 1)]
        [TestCase(1.25, 1.5)]
        [TestCase(1.26, 1.5)]
        [TestCase(1.75, 2)]

        public void RoundingShould_GiveTheCorrectValue(double input, double expected)
        {
            Assert.That(Domain.Utilites.Utility.RoundToTheNearestHalf(input), Is.EqualTo(expected));
        }
    }
}
