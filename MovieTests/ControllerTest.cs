using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MovieAPI.Controllers;
using MovieAPI.Interfaces;
using MovieAPI.Models;
using MovieAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using Xunit;
using DTO = MovieAPI.Models.DTOs;
using Entity = MovieAPI.Models.Entities;

namespace MovieTests
{
    public class ControllerTest
    {
        // to do add tests for null parmeters
        private readonly MoviesController _controller;
        private readonly Mock<ILogger<MoviesController>> _loggerMOQ = new();
        private readonly Mock<IMovieService> _movieMOQ = new();
        private readonly Mock<IMapper> _mapper = new();


        public static IEnumerable<object[]> invalidReviewSubmissions = new List<object[]>()
                                                {
                                                    new object[]{ 0, 0, 1},
                                                    new object[]{ 0, 1, 1},
                                                    new object[]{ 1, 0, 1},
                                                };

        public ControllerTest()
        {
            _controller = new MoviesController(_loggerMOQ.Object, _movieMOQ.Object, _mapper.Object);
        }

        [Fact]
        public void CreationWithNullLogger_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesController(null, _movieMOQ.Object, _mapper.Object));
        }

        [Fact]
        public void CreationWithNullService_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesController(_loggerMOQ.Object, null, _mapper.Object));
        }

        [Fact]
        public void CreationWithNullMapper_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesController(_loggerMOQ.Object, _movieMOQ.Object, null));
        }

        [Fact]
        public void GetShould_ReturnBadRequest_WithInvalidSearchCriteria()
        {
            var result = _controller.Get(new MovieSearchCriteria() { });
            Assert.Equal("BadRequestObjectResult", result.Result.GetType().Name);
        }


        [Fact]
        public void GetShould_LogErrorMessageWithInvalidSearchCriteria()
        {
            _controller.Get(new MovieSearchCriteria() { });

            _loggerMOQ.Verify(logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Bad request was recieved" && @type.Name == "FormattedLogValues"),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
        }


        [Fact]
        public void GetShould_ReturnNotFound_WhenNoDataFound()
        {
            var sc = new MovieSearchCriteria() { Title = "movie" };
            _movieMOQ.Setup(x => x.GetMatchingMovies(It.IsAny<MovieSearchCriteria>())).Returns(new List<Entity.Movie>());

            var result = _controller.Get(sc);

            Assert.Equal("NotFoundObjectResult", result.Result.GetType().Name);
        }


        [Fact]
        public void GetShould_ReturnOkResult_WhenDataFound()
        {
            var sc = new MovieSearchCriteria() { Title = "movie" };
            _movieMOQ.Setup(x => x.GetMatchingMovies(It.IsAny<MovieSearchCriteria>())).Returns(new List<Entity.Movie>()
                                                    {
                                                        new Entity.Movie("supermovie",2012,99,"action")

                                                    }); ;
            var result = (_controller.Get(sc));
            Assert.Equal("OkObjectResult", result.Result.GetType().Name);
        }


        [Fact]
        public void GETShould_Return500OnException()
        {
            var sc = new MovieSearchCriteria() { Title = "movie" };
            _movieMOQ.Setup(x => x.GetMatchingMovies(It.IsAny<MovieSearchCriteria>())).Throws(new Exception("Serious Error Encountered"));

            var result = _controller.Get(sc);

            Assert.Equal("ObjectResult", result.Result.GetType().Name);
        }


        [Fact]
        public void TopFiveByAllRatingsShould_Return404WhenNoDataFound()
        {
            _movieMOQ.Setup(x => x.GetTopMovies(5)).Returns(new List<DTO.MovieResultsList>());

            var result = _controller.TopFiveByAllRatings(5);

            Assert.Equal(typeof(NotFoundObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void TopFiveByAllRatingsShould_Return200WhenDataFound()
        {
            _movieMOQ.Setup(x => x.GetTopMovies(5)).Returns(new List<MovieResultsList>()
                                                    {
                                                        new MovieResultsList()
                                                        {
                                                         MovieTitle = "Super funny Movie"
                                                         , RunningTime = 180
                                                         , Genres = "Superhero"
                                                        }


                                                    }); ;

            var result = _controller.TopFiveByAllRatings(5);

            Assert.Equal(typeof(OkObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void TopFiveByAllRatingsShould_Return500OnException()
        {
            _movieMOQ.Setup(x => x.GetTopMovies(5)).Throws(new Exception("Serious Error Encountered"));
            var result = _controller.TopFiveByAllRatings(5);
            Assert.Equal(typeof(ObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void TopFiveMoviesByReviewerShould_Return400WithInvalidSearchCriteria()
        {
            var result = _controller.TopFiveMoviesByReviewer(0);
            Assert.Equal(typeof(ObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void TopFiveMoviesByReviewerShould_Return404WhenNoDataFound()
        {
            _movieMOQ.Setup(x => x.GetTopFiveMoviesByReviewer(It.IsAny<int>())).Returns(new List<MovieResultsList>());

            var result = _controller.TopFiveMoviesByReviewer(1);

            Assert.Equal(typeof(NotFoundObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void TopFiveMoviesByReviewerShould_Return200WhenDataFound()
        {
            _movieMOQ.Setup(x => x.GetTopFiveMoviesByReviewer(It.IsAny<int>())).Returns(new List<MovieResultsList>()
                                                    {
                                                        new MovieResultsList()
                                                        {
                                                             MovieTitle= "supermovie",
                                                             YearOfRelease =  2012,
                                                             RunningTime = 99,
                                                             Genres = "action"
                                                        }
                                                    }); ;

            var result = _controller.TopFiveMoviesByReviewer(1);

            Assert.Equal(typeof(OkObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void TopFiveMoviesByReviewerShould_Return500OnException()
        {
            _movieMOQ.Setup(x => x.GetTopFiveMoviesByReviewer(It.IsAny<int>())).Throws(new Exception("Serious Error Encountered"));
            var result = _controller.TopFiveMoviesByReviewer(1);
            Assert.Equal(typeof(ObjectResult).Name, result.Result.GetType().Name);
        }


        [Theory]
        [MemberData(nameof(invalidReviewSubmissions))]
        public void AddReviewShould_ReturnBadRequest_WithInvalidReview(int reviewerId, int movieId, int score)
        {
            var result = _controller.AddReview(new AddUpdateReview() { ReviewerId = reviewerId, MovieId = movieId, Score = score });
            Assert.Equal(typeof(BadRequestObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void AddReviewShould_ReturnBadRequest_WhenMovieDoesNotExist()
        {
            _movieMOQ.Setup(x => x.GetMovieById(It.IsAny<int>())).Returns((Entity.Movie)null);
            var result = _controller.AddReview(new AddUpdateReview() { ReviewerId = 1, MovieId = 1934442, Score = 5 });
            Assert.Equal(typeof(NotFoundObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void AddReviewShould_ReturnNotFound_WhenReviewerDoesNotExist()
        {
            _movieMOQ.Setup(x => x.GetMovieById(It.IsAny<int>())).Returns(new Entity.Movie() { Id = 1 });
            _movieMOQ.Setup(x => x.GetReviewerById(23432434)).Returns((Entity.Reviewer)null);
            var result = _controller.AddReview(new AddUpdateReview() { ReviewerId = 23432434, MovieId = 1, Score = 4 });
            Assert.Equal(typeof(NotFoundObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void AddReviewShould_Return500_WhenNotABleToUpdate()
        {
            _movieMOQ.Setup(x => x.GetMovieById(It.IsAny<int>())).Returns(new Entity.Movie() { Id = 1 });
            _movieMOQ.Setup(x => x.GetReviewerById(It.IsAny<int>())).Returns(new Entity.Reviewer() { Id = 1 });
            _movieMOQ.Setup(x => x.AddUpdateReview(It.IsAny<AddUpdateReview>())).Returns(false);
            var result = _controller.AddReview(new AddUpdateReview() { ReviewerId = 1, MovieId = 1, Score = 4 });
            Assert.Equal(typeof(ObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void AddReviewShould_Return200WhenAddedOrUpdated()
        {
            _movieMOQ.Setup(x => x.GetMovieById(It.IsAny<int>())).Returns(new Entity.Movie() { Id = 2 });
            _movieMOQ.Setup(x => x.GetReviewerById(It.IsAny<int>())).Returns(new Entity.Reviewer() { Id = 2 });
            _movieMOQ.Setup(x => x.AddUpdateReview(It.IsAny<AddUpdateReview>())).Returns(true);
            var result = _controller.AddReview(new AddUpdateReview() { MovieId = 2, ReviewerId = 2, Score = 3 });
            Assert.Equal(typeof(OkObjectResult).Name, result.Result.GetType().Name);
        }
    }
}
