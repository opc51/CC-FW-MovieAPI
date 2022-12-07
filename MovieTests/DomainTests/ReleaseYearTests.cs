using MovieAPI.Models.Entities.Common;
using System;
using Xunit;

namespace MovieTests.DomainTests
{
    public class ReleaseYearTests
    {
        [Theory]
        [InlineData(1895)]
        [InlineData(2022)]
        public void CreatesReleaseYear_WhenData_IsValid(int year)
        {
            var sut = ReleaseYear.Create(year);
            Assert.Equal(year, sut.Value);
        }

        [Theory]
        [InlineData(1894)]
        [InlineData(2122)]
        public void ThrowsException_WhenData_IsValid(int year)
        {
            Assert.Throws<ArgumentException>(() => ReleaseYear.Create(year));
        }

        [Fact]
        public void ImplicityConversion_FromReleaseYearToInt_Sucessful()
        {
            int year = 2000;
            ReleaseYear releaseYear = ReleaseYear.Create(year);
            int sut = releaseYear;
            Assert.Equal(typeof(int), sut.GetType());
        }

        [Fact]
        public void ExplicitConversion_FromIntToReleaseYearSuccessful()
        {
            int year = 2000;
            ReleaseYear sut = (ReleaseYear) year;
            Assert.Equal(typeof(ReleaseYear), sut.GetType());
        }
    }
}
