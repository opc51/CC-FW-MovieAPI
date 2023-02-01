using AutoFixture;
using MovieAPI.Models.Domain;
using MovieAPI.Models.Domain.Common;
using System;
using System.Collections.Generic;
using Xunit;

namespace MovieTests.DomainTests
{
    public class MovieTests
    {
        private Fixture _fixture = new();

        [Fact]
        public void MovieSetUp_ValidData_Succeeds()
        {
            string TITLE = _fixture.Create<string>();
            var movie = Movie.Create(TITLE, 1974, (RunningTime) 123, MovieAPI.Models.Enum.GenreType.Action);
            var reviewList = new List<Review>()
            {
                Review.Create(1, 1, 2),
                Review.Create(1, 1, 3),
                Review.Create(1, 1, 4)
            };
            movie.AddReviews(reviewList);

            //Assert.Equal(3, movie.Reviews.Count);
            Assert.Equal(TITLE, movie.Title);
            //Assert.Equal(3, movie.GetAverageScore);
        }

        [Fact]
        public void EmptyOrNullReviews_ThrowsException()
        {
            string TITLE = _fixture.Create<string>();
            var movie = Movie.Create(TITLE, 1974, (RunningTime) 123, MovieAPI.Models.Enum.GenreType.Action);
            var reviewList = new List<Review>();

            Assert.Throws<ArgumentException>(() => movie.AddReviews(reviewList));
        }
    }
}
