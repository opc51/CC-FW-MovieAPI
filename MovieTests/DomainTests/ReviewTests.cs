using AutoFixture;
using MovieAPI.Models.Entities;
using System;
using Xunit;

namespace MovieTests.DomainTests
{
    public class ReviewTests
    {
        private Fixture fixture = new();

        #region CreateReview

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void CreateReview_Should_AllowsValuesBetween1and5(int score)
        {
            Review sut = Review.Create(1, 1, score);
            Assert.Equal(score, sut.Score);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        public void CreateReviewShould_NotAllowsValuesOutside1and5(int score)
        {   
            Assert.Throws<ArgumentOutOfRangeException>(() => Review.Create(fixture.Create<int>(), fixture.Create<int>(), score));
        }

        [Fact]
        public void CreateReviewerId_AsZero_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Review.Create(0, fixture.Create<int>(), 1));
        }

        [Fact]
        public void CreateReview_MovieIdAsZero_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Review.Create(fixture.Create<int>(), 0, 1));
        }

        #endregion

        #region 

        [Fact]
        public void UpdateReview_ScoreOutfFBounds_ThrowsArgumentOutOfRangeException()
        {
            var validMovie = Review.Create(fixture.Create<int>(), 1, 1);

            Assert.Throws<ArgumentOutOfRangeException>(() => { validMovie.Score = 6; });
        }


        [Fact]
        public void UpdateReview_MovieIdOutfFBounds_ThrowsArgumentOutOfRangeException()
        {
            var validReview = Review.Create(fixture.Create<int>(), 1, 1);

            Assert.Throws<ArgumentOutOfRangeException>(() => { validReview.MovieId = -122; });
        }

        [Fact]
        public void UpdateReview_ReviewerIdOutfFBounds_ThrowsArgumentOutOfRangeException()
        {
            var validReview = Review.Create(fixture.Create<int>(), 1, 1);

            Assert.Throws<ArgumentOutOfRangeException>(() => { validReview.ReviewerId = -122; });
        }

        #endregion

    }
}
