using MovieAPI.Models.Entities;
using System;
using System.Collections.Generic;
using Xunit;

namespace MovieTests.DomainTests
{
    public class MovieTests
    {
        [Fact]
        public void MovieSetUp_ValidData_Succeeds() {
            const string TITLE = "Enter The Dragon";
            var movie = Movie.Create(TITLE, 1974, 123, MovieAPI.Models.Enum.GenreType.Action);
            var reviewList = new List<Review>()
            {
                Review.Create(1, 1, 2),
                Review.Create(1, 1, 3),
                Review.Create(1, 1, 4)
            };
            movie.AddReviews(reviewList);

            Assert.Equal(3, movie.Reviews.Count);
            Assert.Equal(TITLE, movie.Title);
            Assert.Equal(3, movie.GetAverageScore);
        }

        [Fact]
        public void EmptyOrNullReviews_ThrowsException()
        {
            const string TITLE = "Enter The Dragon";
            var movie = Movie.Create(TITLE, 1974, 123, MovieAPI.Models.Enum.GenreType.Action);
            var reviewList = new List<Review>();

            Assert.Throws<ArgumentException>(() => movie.AddReviews(reviewList));

        }


    }
}
