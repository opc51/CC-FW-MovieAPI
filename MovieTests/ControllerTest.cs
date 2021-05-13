using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MovieAPI.Controllers;
using MovieAPI.Interfaces;
using MovieAPI.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace MovieTests
{
    public class ControllerTest
    {

        private readonly MoviesController _controller;
        private readonly Mock<ILogger<MoviesController>> _loggerMOQ = new();
        private readonly Mock<IMovieService> _movieMOQ = new();


        public static IEnumerable<object[]> invalidReviewSubmissions = new List<object[]>()
                                                {
                                                    new object[]{ 0, 0, 1},
                                                    new object[]{ 0, 1, 1},
                                                    new object[]{ 1, 0, 1},
                                                };


        public ControllerTest()
        {
            _controller = new MoviesController(_loggerMOQ.Object, _movieMOQ.Object);
        }


        [Fact]
        public void GetShould_Return400WithInvalidSearchCriteria()
        {
            var result = (ObjectResult)_controller.Get(new MovieSearchCriteria() { });
            Assert.Equal(400, result.StatusCode);
        }


        [Fact]
        public void GetShould_Return404WhenNoDataFound()
        {
            var sc = new MovieSearchCriteria() { Title = "movie" };
            _movieMOQ.Setup(x => x.GetMatchingMovies(It.IsAny<MovieSearchCriteria>())).Returns(new List<Movie>());

            var result = (ObjectResult)_controller.Get(sc);

            Assert.Equal(404, result.StatusCode);
        }


        [Fact]
        public void GetShould_Return200WhenDataFound()
        {
            var sc = new MovieSearchCriteria() { Title = "movie" };
            _movieMOQ.Setup(x => x.GetMatchingMovies(It.IsAny<MovieSearchCriteria>())).Returns(new List<Movie>()
                                                    {
                                                        new Movie("supermovie",2012,99,"action")

                                                    }); ;

            var result = (ObjectResult)_controller.Get(sc);

            Assert.Equal(200, result.StatusCode);
        }


        [Fact]
        public void GETShould_Return500OnException()
        {
            var sc = new MovieSearchCriteria() { Title = "movie" };
            _movieMOQ.Setup(x => x.GetMatchingMovies(It.IsAny<MovieSearchCriteria>())).Throws(new Exception("Serious Error Encountered"));
            var result = (ObjectResult)_controller.Get(sc);
            Assert.Equal(500, result.StatusCode);
        }


        [Fact]
        public void TopFiveByAllRatingsShould_Return404WhenNoDataFound()
        {
            _movieMOQ.Setup(x => x.GetTopFiveMovies()).Returns(new List<ResultList>());

            var result = (ObjectResult)_controller.TopFiveByAllRatings();

            Assert.Equal(404, result.StatusCode);
        }


        [Fact]
        public void TopFiveByAllRatingsShould_Return200WhenDataFound()
        {
            _movieMOQ.Setup(x => x.GetTopFiveMovies()).Returns(new List<ResultList>()
                                                    {
                                                        new ResultList()
                                                        {
                                                         MovieTitle = "Super funny Movie"
                                                         , RunningTime = 180
                                                         , Genres = "Superhero"
                                                        }


                                                    }); ;

            var result = (ObjectResult)_controller.TopFiveByAllRatings();

            Assert.Equal(200, result.StatusCode);
        }


        [Fact]
        public void TopFiveByAllRatingsShould_Return500OnException()
        {
            _movieMOQ.Setup(x => x.GetTopFiveMovies()).Throws(new Exception("Serious Error Encountered"));
            var result = (ObjectResult)_controller.TopFiveByAllRatings();
            Assert.Equal(500, result.StatusCode);
        }


        [Fact]
        public void TopFiveMoviesByReviewerShould_Return400WithInvalidSearchCriteria()
        {
            var result = (ObjectResult)_controller.TopFiveMoviesByReviewer(0);
            Assert.Equal(400, result.StatusCode);
        }


        [Fact]
        public void TopFiveMoviesByReviewerShould_Return404WhenNoDataFound()
        {
 
            _movieMOQ.Setup(x => x.GetTopFiveMoviesByReviewer(It.IsAny<int>())).Returns(new List<ResultList>());

            var result = (ObjectResult)_controller.TopFiveMoviesByReviewer(1);

            Assert.Equal(404, result.StatusCode);
        }


        [Fact]
        public void TopFiveMoviesByReviewerShould_Return200WhenDataFound()
        {
            _movieMOQ.Setup(x => x.GetTopFiveMoviesByReviewer(It.IsAny<int>())).Returns(new List<ResultList>()
                                                    {
                                                        new ResultList()
                                                        {
                                                             MovieTitle= "supermovie",
                                                             YearOfRelease =  2012,
                                                             RunningTime = 99,
                                                             Genres = "action"
                                                        }
                                                    }); ;

            var result = (ObjectResult)_controller.TopFiveMoviesByReviewer(1);

            Assert.Equal(200, result.StatusCode);
        }


        [Fact]
        public void TopFiveMoviesByReviewerShould_Return500OnException()
        {
            _movieMOQ.Setup(x => x.GetTopFiveMoviesByReviewer(It.IsAny<int>())).Throws(new Exception("Serious Error Encountered"));
            var result = (ObjectResult)_controller.TopFiveMoviesByReviewer(1);
            Assert.Equal(500, result.StatusCode);
        }


        [Theory]
        [MemberData(nameof(invalidReviewSubmissions))]
        public void AddReviewShould_Return400WithInvalidReview(int reviewerId, int movieId, int score)
        {
            var result = (ObjectResult)_controller.AddReview(new AddUpdateReview() { ReviewerId = reviewerId, MovieId = movieId, Score = score });
            Assert.Equal(400, result.StatusCode);
        }


        [Fact]
        public void AddReviewShould_Return404WhenMovieDoesNotExist()
        {
            _movieMOQ.Setup(x => x.GetMovieById(It.IsAny<int>())).Returns((Movie) null);
            var result = (ObjectResult)_controller.AddReview(new AddUpdateReview() { ReviewerId = 1, MovieId = 1934442, Score = 5 });
            Assert.Equal(404, result.StatusCode);
        }


        [Fact]
        public void AddReviewShould_Return404WhenReviewerDoesNotExist()
        {
            _movieMOQ.Setup(x => x.GetMovieById(It.IsAny<int>())).Returns(new Movie() { Id = 1 });
            _movieMOQ.Setup(x => x.GetReviewerById(23432434)).Returns((Reviewer) null);
            var result = (ObjectResult)_controller.AddReview(new AddUpdateReview() { ReviewerId = 23432434, MovieId = 1, Score = 4 });
            Assert.Equal(404, result.StatusCode);
        }


        [Fact]
        public void AddReviewShould_Return500WhenNotABleToUpdate()
        {
            _movieMOQ.Setup(x => x.GetMovieById(It.IsAny<int>())).Returns(new Movie() { Id = 1 });
            _movieMOQ.Setup(x => x.GetReviewerById(It.IsAny<int>())).Returns(new Reviewer() { Id = 1 });
            _movieMOQ.Setup(x => x.AddUpdateReview(It.IsAny<AddUpdateReview>())).Returns(false);
            var result = (ObjectResult)_controller.AddReview(new AddUpdateReview() { ReviewerId = 1, MovieId = 1, Score = 4 });
            Assert.Equal(500, result.StatusCode);
        }


        [Fact]
        public void AddReviewShould_Return200WhenAddedOrUpdated()
        {
            _movieMOQ.Setup(x => x.GetMovieById(It.IsAny<int>())).Returns(new Movie() { Id = 2 });
            _movieMOQ.Setup(x => x.GetReviewerById(It.IsAny<int>())).Returns(new Reviewer() { Id = 2 });
            _movieMOQ.Setup(x =>  x.AddUpdateReview(It.IsAny<AddUpdateReview>())).Returns(true);
            var result = (ObjectResult)_controller.AddReview(new AddUpdateReview() { MovieId = 2, ReviewerId = 2, Score = 3 });
            Assert.Equal(200, result.StatusCode);
        }




    }
}
