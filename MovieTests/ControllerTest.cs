using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Entity = Movie.Repository.Entities;
using Output = Movie.Repository.Services.DTOs.Output;
using Movie.API.Controllers;
using Movie.Repository.Services;
using Movie.Respository.Services;
using Movie.Repository.Entities.Common;
using Movie.Repository.Entities.Enum;
using Movie.API.AutoMapper;
using NUnit.Framework;

namespace MovieTests
{
    public class ControllerTest : InMemoryDatabaseFixture
    {
        /// <summary>
        /// A controller that uses in memory data. Used in most test.
        /// 
        /// Suitable for tets that require data to be exercised
        /// </summary>
        private readonly MoviesController _inMemoryController;
        /// <summary>
        /// A movie service for retiriving data from real world objects
        /// </summary>
        private readonly MovieService _movieService;
        /// <summary>
        /// A controller that doesn't use any data. Mainly used in failure tests 
        /// </summary>
        private readonly MoviesController _mockedController;
        /// <summary>
        /// A mock logger
        /// </summary>
        private readonly Mock<ILogger<MoviesController>> _loggerMOQ = new();
        /// <summary>
        /// A mock Movie Service needed by the mocked
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

        private readonly Fixture _fixture = new();


        public readonly static IEnumerable<object[]> invalidReviewSubmissions = new List<object[]>()
                                                {
                                                    new object[]{ 0, 0, 1},
                                                    new object[]{ 0, 1, 1},
                                                    new object[]{ 1, 0, 1},
                                                };

        public ControllerTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MovieProfiles>();
            });

            _mapper = config.CreateMapper();

            _movieService = new MovieService(_database);

            _inMemoryController = new MoviesController(_loggerMOQ.Object, _movieService, _mapper, _senderMOQ.Object);

            _mockedController = new MoviesController(_loggerMOQ.Object, _movieMOQ.Object, _mapperMOQ.Object, _senderMOQ.Object);
        }

        #region InitialisationTests

        [Test]
        public void CreationWithNullLogger_ThrowsArgumentNulException()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesController(null, _movieService, _mapperMOQ.Object, _senderMOQ.Object));
        }

        [Test]
        public void CreationWithNullService_ThrowsArgumentNulException()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesController(_loggerMOQ.Object, null, _mapperMOQ.Object, _senderMOQ.Object));
        }

        [Test]
        public void CreationWithNullMapper_ThrowsArgumentNulException()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesController(_loggerMOQ.Object, _movieService, null, _senderMOQ.Object));
        }

        [Test]
        public void CreationWithNullSender_ThrowsArgumentNulException()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesController(_loggerMOQ.Object, _movieService, _mapperMOQ.Object, null));
        }

        #endregion

        #region GetMethodTests

        [Test]
        public void GetShould_ReturnBadRequest_WithInvalidQuery()
        {
            var result = _inMemoryController.Get(new GetMoviesQuery(), new CancellationToken());
            result.Result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void GetShould_ReturnNotFound_WhenNoDataFound()
        {
            var sc = new GetMoviesQuery() { Title = _fixture.Create<string>() };
            var result = _inMemoryController.Get(sc, new CancellationToken());
            result.Result.Result.Should().BeOfType<NotFoundObjectResult>();
        }


        [Test]
        public void GetShould_ReturnOkResult_WhenDataFound()
        {
            var sc = new GetMoviesQuery() { Title = "movie" };
            var data = new List<Output.Movie>() {
                    new Output.Movie() { Title = "Super Fun Movie 1" }
                    ,new Output.Movie() { Title = "Super Fun Movie 2" }
            };
            _senderMOQ.Setup(x => x.Send(It.IsAny<GetMoviesQuery>(), It.IsAny<CancellationToken>()).Result)
                .Returns(data);
            var result = _inMemoryController.Get(sc, new CancellationToken());

            result.Result.Result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region TopRatedTests

        [Test]
        public void TopRated_Should_Return404WhenNoDataFound()
        {
            _movieMOQ.Setup(x => x.GetTopMovies(5)).Returns(new List<Output.MovieResult>());
            var result = _mockedController.TopRatedMovies(5);
            Assert.That(typeof(NotFoundObjectResult).Name, Is.EqualTo(result.Result.GetType().Name));
        }


        [Test]
        public void TopRated_Should_Return200WhenDataFound()
        {
            var result = _inMemoryController.TopRatedMovies(5);
            Assert.That(typeof(OkObjectResult).Name, Is.EqualTo(result.Result.GetType().Name));
        }


        [Test]
        public void TopRated_Should_Return500OnException()
        {
            _movieMOQ.Setup(x => x.GetTopMovies(5)).Throws(new Exception("Serious Error Encountered"));
            var result = _mockedController.TopRatedMovies(5);
            Assert.That(typeof(ObjectResult).Name, Is.EqualTo(result.Result.GetType().Name));
        }

        #endregion

        #region TopFiveByReviewerTests

        [TestCase(5, 0)] // invalid reviewerID
        [TestCase(0, 2)] // Invalid number of movies
        [TestCase(0, 0)] // both invalid
        public void TopFiveMoviesByReviewerShould_Return400_WithZeroReviewerId(int numberOfMovies, int reviewerId)
        {
            var result = _inMemoryController.TopRankedMoviesByReviewer(numberOfMovies, reviewerId);
            Assert.That(typeof(ObjectResult).Name, Is.EqualTo(result.Result.GetType().Name));
        }

        [Test]
        public void TopFiveMoviesByReviewerShould_Return404WhenNoDataFound()
        {
            var result = _inMemoryController.TopRankedMoviesByReviewer(5, 666);
            Assert.That(typeof(NotFoundObjectResult).Name, Is.EqualTo(result.Result.GetType().Name));
        }


        [Test]
        public void TopFiveMoviesByReviewerShould_Return200WhenDataFound()
        {
            var result = _inMemoryController.TopRankedMoviesByReviewer(3, 1);
            Assert.That(typeof(OkObjectResult).Name, Is.EqualTo(result.Result.GetType().Name));
        }


        [Test]
        public void TopFiveMoviesByReviewerShould_Return500OnException()
        {
            _movieMOQ.Setup(x => x.GetMoviesByReviewer(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Serious Error Encountered"));
            var result = _mockedController.TopRankedMoviesByReviewer(5, 1);
            Assert.That(typeof(ObjectResult).Name, Is.EqualTo(result.Result.GetType().Name));
        }

        #endregion

        #region AddReviewTests

        [TestCaseSource(nameof(invalidReviewSubmissions))]
        public void AddReviewShould_ReturnBadRequest_WithInvalidReview(int reviewerId, int movieId, int score)
        {
            var result = _inMemoryController.AddReview(new AddUpdateReview() { ReviewerId = reviewerId, MovieId = movieId, Score = score });
            Assert.That(typeof(BadRequestObjectResult).Name, Is.EqualTo(result.Result.GetType().Name));
        }


        [Test]
        public void AddReviewShould_ReturnBadRequest_WhenMovieDoesNotExist()
        {
            var result = _inMemoryController.AddReview(new AddUpdateReview() { ReviewerId = 1, MovieId = 1934442, Score = 5 });
            Assert.That(typeof(NotFoundObjectResult).Name, Is.EqualTo(result.Result.GetType().Name));
        }


        [Test]
        public void AddReviewShould_ReturnNotFound_WhenReviewerDoesNotExist()
        {
            var result = _inMemoryController.AddReview(new AddUpdateReview() { ReviewerId = 23432434, MovieId = 1, Score = 4 });
            Assert.That(typeof(NotFoundObjectResult).Name, Is.EqualTo(result.Result.GetType().Name));
        }


        [Test]
        public void AddReviewShould_Return500_WhenNotABleToUpdate()
        {
            ReleaseYear validReleaseYear = 2012;
            var movieOne = Entity.Movie.Create(_fixture.Create<string>(), validReleaseYear,
                                               _fixture.Create<RunningTime>(), _fixture.Create<GenreType>());
            _movieMOQ.Setup(x => x.GetMovieById(It.IsAny<int>())).Returns(movieOne);

            _movieMOQ.Setup(x => x.GetReviewerById(It.IsAny<int>()))
                            .Returns(Entity.Reviewer.Create(_fixture.Create<string>(), "adminuser@freewheel.com", "gb", "01689817516", null));
            _movieMOQ.Setup(x => x.AddUpdateReview(It.IsAny<AddUpdateReview>())).Returns(false);
            var result = _mockedController.AddReview(new AddUpdateReview() { ReviewerId = 1, MovieId = 1, Score = 4 });
            Assert.That(typeof(ObjectResult).Name, Is.EqualTo(result.Result.GetType().Name));
            _movieMOQ.Reset();
        }


        [Test]
        public void AddReviewShould_Return200WhenAddedOrUpdated()
        {
            var result = _inMemoryController.AddReview(new AddUpdateReview() { MovieId = 2, ReviewerId = 2, Score = 3 });
            Assert.That(typeof(OkObjectResult).Name, Is.EqualTo(result.Result.GetType().Name));
        }
        #endregion
    }
}
