using AutoFixture;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MovieAPI.Controllers;
using MovieAPI.Interfaces;
using MovieAPI.Models;
using MovieAPI.Models.DTOs.Outputs;
using MovieAPI.Profiles;
using MovieAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using Entity = MovieAPI.Models.Entities;

namespace MovieTests
{
    [Collection("In Memory Database Collection")]
    public class ControllerTest
    {
        /// <summary>
        /// A controller that uses in memory data. Used in most test
        /// </summary>
        private readonly MoviesController _inMemoryController;
        /// <summary>
        /// A movie service for retiriving data from real world objects
        /// </summary>
        private MovieService _movieService;
        /// <summary>
        /// A controller that uses in memory data. 
        /// </summary>
        private readonly MoviesController _mockedController;
        /// <summary>
        /// A mock logger
        /// </summary>
        private readonly Mock<ILogger<MoviesController>> _loggerMOQ = new();
        /// <summary>
        /// A mock Movie Service
        /// </summary>
        private readonly Mock<IMovieService> _movieMOQ = new();
        /// <summary>
        /// A mock AUTO mapper
        /// </summary>
        private readonly Mock<IMapper> _mapperMOQ = new();
        /// <summary>
        /// A mock Mediatr Sender
        /// </summary>
        private readonly Mock<ISender> _senderMOQ = new();
        /// <summary>
        /// A real Automapper to work on
        /// </summary>
        private readonly IMapper _mapper;

        private Fixture fixture = new();


        public static IEnumerable<object[]> invalidReviewSubmissions = new List<object[]>()
                                                {
                                                    new object[]{ 0, 0, 1},
                                                    new object[]{ 0, 1, 1},
                                                    new object[]{ 1, 0, 1},
                                                };

        public ControllerTest(InMemoryDatabaseFixture fixture)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MovieProfiles>();
            });

            _mapper = config.CreateMapper();


            _movieService = new MovieService(fixture._database);

            _inMemoryController = new MoviesController(_loggerMOQ.Object, _movieService, _mapper, _senderMOQ.Object);

            _mockedController = new MoviesController(_loggerMOQ.Object, _movieMOQ.Object, _mapperMOQ.Object, _senderMOQ.Object);
        }

        [Fact]
        public void CreationWithNullLogger_ThrowsArgumentNulException()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesController(null, _movieService, _mapperMOQ.Object, _senderMOQ.Object));
        }

        [Fact]
        public void CreationWithNullService_ThrowsArgumentNulException()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesController(_loggerMOQ.Object, null, _mapperMOQ.Object, _senderMOQ.Object));
        }

        [Fact]
        public void CreationWithNullMapper_ThrowsArgumentNulException()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesController(_loggerMOQ.Object, _movieService, null, _senderMOQ.Object));
        }

        [Fact]
        public void GetShould_ReturnBadRequest_WithInvalidSearchCriteria()
        {
            var result = _inMemoryController.Get(new MovieSearchCriteria() { }, new CancellationToken());
            Assert.Equal("BadRequestObjectResult", result.Result.GetType().Name);
        }

        [Fact]
        public void GetShould_LogErrorMessageWithInvalidSearchCriteria()
        {
            _inMemoryController.Get(new MovieSearchCriteria() { }, new CancellationToken());

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
            var sc = new MovieSearchCriteria() { Title = fixture.Create<string>() };

            var result = _inMemoryController.Get(sc, new CancellationToken());

            Assert.Equal("NotFoundObjectResult", result.Result.GetType().Name);
        }


        [Fact]
        public void GetShould_ReturnOkResult_WhenDataFound()
        {
            var sc = new MovieSearchCriteria() { Title = "movie" };
            var result = (_inMemoryController.Get(sc, new CancellationToken()));
            Assert.Equal("OkObjectResult", result.Result.GetType().Name);
        }


        [Fact]
        public void GETShould_Return500OnException()
        {
            var sc = new MovieSearchCriteria() { Title = "movie" };
            _movieMOQ.Setup(x => x.GetMatchingMovies(It.IsAny<MovieSearchCriteria>())).Throws(new Exception("Serious Error Encountered"));

            var result = _mockedController.Get(sc, new CancellationToken());

            Assert.Equal("ObjectResult", result.Result.GetType().Name);
        }


        [Fact]
        public void TopRated_Should_Return404WhenNoDataFound()
        {
            _movieMOQ.Setup(x => x.GetTopMovies(5)).Returns(new List<MovieResultsList>());
            var result = _mockedController.TopRatedMovies(5);
            Assert.Equal(typeof(NotFoundObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void TopRated_Should_Return200WhenDataFound()
        {
            var result = _inMemoryController.TopRatedMovies(5);
            Assert.Equal(typeof(OkObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void TopRated_Should_Return500OnException()
        {
            _movieMOQ.Setup(x => x.GetTopMovies(5)).Throws(new Exception("Serious Error Encountered"));
            var result = _mockedController.TopRatedMovies(5);
            Assert.Equal(typeof(ObjectResult).Name, result.Result.GetType().Name);
        }


        [Theory]
        [InlineData(5, 0)] // invalid reviewerID
        [InlineData(0, 2)] // Invalid number of movies
        [InlineData(0, 0)] // both invalid
        public void TopFiveMoviesByReviewerShould_Return400_WithZeroReviewerId(int numberOfMovies, int reviewerId)
        {
            var result = _inMemoryController.TopRankedMoviesByReviewer(numberOfMovies, reviewerId);
            Assert.Equal(typeof(ObjectResult).Name, result.Result.GetType().Name);
        }

        [Fact]
        public void TopFiveMoviesByReviewerShould_Return404WhenNoDataFound()
        {
            var result = _inMemoryController.TopRankedMoviesByReviewer(5, 666);
            Assert.Equal(typeof(NotFoundObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void TopFiveMoviesByReviewerShould_Return200WhenDataFound()
        {
            var result = _inMemoryController.TopRankedMoviesByReviewer(3, 1);
            Assert.Equal(typeof(OkObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void TopFiveMoviesByReviewerShould_Return500OnException()
        {
            _movieMOQ.Setup(x => x.GetMoviesByReviewer(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Serious Error Encountered"));
            var result = _mockedController.TopRankedMoviesByReviewer(5, 1);
            Assert.Equal(typeof(ObjectResult).Name, result.Result.GetType().Name);
        }


        [Theory]
        [MemberData(nameof(invalidReviewSubmissions))]
        public void AddReviewShould_ReturnBadRequest_WithInvalidReview(int reviewerId, int movieId, int score)
        {
            var result = _inMemoryController.AddReview(new AddUpdateReview() { ReviewerId = reviewerId, MovieId = movieId, Score = score });
            Assert.Equal(typeof(BadRequestObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void AddReviewShould_ReturnBadRequest_WhenMovieDoesNotExist()
        {
            var result = _inMemoryController.AddReview(new AddUpdateReview() { ReviewerId = 1, MovieId = 1934442, Score = 5 });
            Assert.Equal(typeof(NotFoundObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void AddReviewShould_ReturnNotFound_WhenReviewerDoesNotExist()
        {
            var result = _inMemoryController.AddReview(new AddUpdateReview() { ReviewerId = 23432434, MovieId = 1, Score = 4 });
            Assert.Equal(typeof(NotFoundObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void AddReviewShould_Return500_WhenNotABleToUpdate()
        {
            _movieMOQ.Setup(x => x.GetMovieById(It.IsAny<int>())).Returns(new Entity.Movie() { Id = 1 });
            _movieMOQ.Setup(x => x.GetReviewerById(It.IsAny<int>())).Returns(new Entity.Reviewer() { Id = 1 });
            _movieMOQ.Setup(x => x.AddUpdateReview(It.IsAny<AddUpdateReview>())).Returns(false);
            var result = _mockedController.AddReview(new AddUpdateReview() { ReviewerId = 1, MovieId = 1, Score = 4 });
            Assert.Equal(typeof(ObjectResult).Name, result.Result.GetType().Name);
        }


        [Fact]
        public void AddReviewShould_Return200WhenAddedOrUpdated()
        {
            var result = _inMemoryController.AddReview(new AddUpdateReview() { MovieId = 2, ReviewerId = 2, Score = 3 });
            Assert.Equal(typeof(OkObjectResult).Name, result.Result.GetType().Name);
        }
    }
}
