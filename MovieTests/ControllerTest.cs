using AutoFixture;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Movie.API.AutoMapper;
using Movie.API.Controllers;
using Movie.Repository.Services;
using Movie.Repository.Services.DTOs.Output;
using Movie.Repository.Services.TopRankedMoviesByReviewer;
using Movie.Repository.Services.TopRatedMovies;
using Movie.Respository.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Output = Movie.Repository.Services.DTOs.Output;

namespace MovieTests
{
    public class ControllerTest : InMemoryDatabaseFixture
    {
        /// <summary>
        /// A controller that uses in memory data. Used in most test.
        /// 
        /// Suitable for tests that require data to be exercised
        /// </summary>
        private readonly MoviesController _inMemoryController;
        /// <summary>
        /// A movie service for retiriving data from real world objects
        /// </summary>
        private readonly MovieService _movieService;
        /// <summary>
        /// A mock logger
        /// </summary>
        private readonly Mock<ILogger<MoviesController>> _loggerMOQ = new();
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

        // To do look at mapper situation - is it even needed?
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
        }

        [TearDown]
        public void TearDown()
        {
            _senderMOQ.Reset();
            _mapperMOQ.Reset();
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
        public void GetShould_ReturnNotFound_WhenNoDataFound()
        {
            var result = _inMemoryController.Get(
                                new GetMoviesQuery(),
                                new CancellationToken());
            result.Result.Result.Should().BeOfType<NotFoundObjectResult>();
        }


        [Test]
        public async Task GetShould_ReturnOkResult_WhenDataFound()
        {
            var data = new List<Output.Movie>() {
                    new Output.Movie() { Title = "Super Fun Movie 1" }
                    ,new Output.Movie() { Title = "Super Fun Movie 2" }
            };

            _senderMOQ.Setup(x => x.Send(It.IsAny<GetMoviesQuery>(), It.IsAny<CancellationToken>()).Result)
                .Returns(data);

            var sut = await _inMemoryController.Get(new GetMoviesQuery(), new CancellationToken());
            sut.Result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region TopRatedTests

        [Test]
        public void TopRated_Should_Return404WhenNoDataFound()
        {
            var sut = _inMemoryController.TopRatedMovies(new GetTopRatedMoviesQuery(), new CancellationToken()).Result;
            sut.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public void TopRated_Should_Return200WhenDataFound()
        {
            var query = new GetTopRatedMoviesQuery() { NumberOfMovies = 5 };
            var data = new List<MovieResult>() {
                new MovieResult { MovieId = _fixture.Create<int>()}
            };

            _senderMOQ.Setup(x => x.Send(query, It.IsAny<CancellationToken>()).Result)
                .Returns(data);

            var sut = _inMemoryController.TopRatedMovies(query, new CancellationToken()).Result;
            sut.Result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region TopFiveByReviewerTests

        [TestCase(5, 0)] // invalid reviewerID
        [TestCase(0, 2)] // Invalid number of movies
        [TestCase(0, 0)] // both invalid
        public void TopFiveMoviesByReviewerShould_Return400_WithZeroReviewerId(int numberOfMovies, int reviewerId)
        {
            var query = new TopRankedMoviesByReviewerQuery()
            {
                NumberOfMovies = numberOfMovies,
                ReviewerId = reviewerId
            };

            var result = _inMemoryController.TopRankedMoviesByReviewer(query, new CancellationToken()).Result;
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public void TopFiveMoviesByReviewerShould_Return404WhenNoDataFound()
        {
            var query = new TopRankedMoviesByReviewerQuery()
            {
                NumberOfMovies = 666,
                ReviewerId = 100,
            };
            var sut = _inMemoryController.TopRankedMoviesByReviewer(query, new CancellationToken()).Result;
            sut.Result.Should().BeOfType<NotFoundObjectResult>();
        }


        [Test]
        public void TopFiveMoviesByReviewerShould_Return200WhenDataFound()
        {
            var query = new TopRankedMoviesByReviewerQuery()
            {
                NumberOfMovies = 1,
                ReviewerId = 3
            };

            var data = new List<MovieResult>() {
                new MovieResult (){ },
            };

            _senderMOQ.Setup(x => x.Send(query, It.IsAny<CancellationToken>()).Result)
                .Returns(data);

            var sut = _inMemoryController.TopRankedMoviesByReviewer(query, new CancellationToken()).Result;
            sut.Result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region AddReviewTests

        [TestCaseSource(nameof(invalidReviewSubmissions))]
        public void AddReviewShould_ReturnBadRequest_WithInvalidReview(int reviewerId, int movieId, int score)
        {
            var sut = _inMemoryController.AddReview(
                                            new AddUpdateReview()
                                            {
                                                ReviewerId = reviewerId,
                                                MovieId = movieId,
                                                Score = score
                                            }).Result;

            sut.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void AddReviewShould_ReturnBadRequest_WhenMovieDoesNotExist()
        {
            var sut = _inMemoryController.AddReview(
                                                new AddUpdateReview()
                                                {
                                                    ReviewerId = 1,
                                                    MovieId = 1934442,
                                                    Score = 5
                                                })
                                                .Result;

            sut.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public void AddReviewShould_ReturnNotFound_WhenReviewerDoesNotExist()
        {
            var sut = _inMemoryController.AddReview(
                                                new AddUpdateReview()
                                                {
                                                    ReviewerId = 23432434,
                                                    MovieId = 1,
                                                    Score = 4
                                                })
                                            .Result;
            sut.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public void AddReviewShould_Return200WhenAddedOrUpdated()
        {
            var sut = _inMemoryController.AddReview(
                                            new AddUpdateReview()
                                            {
                                                MovieId = 2,
                                                ReviewerId = 2,
                                                Score = 3
                                            })
                                           .Result;
            Assert.That(typeof(OkObjectResult).Name, Is.EqualTo(sut.GetType().Name));
            sut.Should().BeOfType<OkObjectResult>();
        }
        #endregion
    }
}
