using AutoFixture;
using MovieAPI.Models.Entities;
using System;
using Xunit;

namespace MovieTests.DomainTests
{
    public class ReviewTests
    {
        private Fixture fixture = new();

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void ReviewShould_AllowsValuesBetween1and5(int score)
        {
            Review sut = Review.Create(1, 1, score);
            Assert.Equal(score, sut.Score);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(7)]
        public void ReviewShould_NotAllowsValuesOutside1and5(int score)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Review.Create(fixture.Create<int>(), fixture.Create<int>(), score));
        }

        [Fact]
        public void ReviewerId_AsZero_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentException>(() => Review.Create(0, fixture.Create<int>(), 1));
        }

        [Fact]
        public void MovieId_AsZero_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentException>(() => Review.Create(fixture.Create<int>(), 0, 1));
        }
    }
}
