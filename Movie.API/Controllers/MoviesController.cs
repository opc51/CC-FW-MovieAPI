using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movie.Repository.Services;
using Movie.Respository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Output = Movie.Repository.Services.DTOs.Output;

namespace Movie.API.Controllers
{
    /// <summary>
    /// Contains methods used to add movie reviews and show lists of highly rated movies
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;
        private readonly ISender _sender;

        /// <summary>
        /// public constructor used to inject dependencies into the Movie Controller
        /// </summary>
        /// <param name="logger">Type of <see cref="ILogger"/></param>
        /// <param name="movieDataService"><see cref="IMovieService"/> used to interact with <see cref="Repository.APIContext"/> </param>
        /// <param name="mapper"><see cref="AutoMapper"/> for converting Domain objects to outbound DTO's</param> 
        /// <param name="sender">Mediatr implementation with <see cref="ISender"/></param> 
        /// <exception cref="ArgumentNullException"></exception>
        public MoviesController(ILogger<MoviesController> logger, IMovieService movieDataService, IMapper mapper, ISender sender)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _movieService = movieDataService ?? throw new ArgumentNullException(nameof(movieDataService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        /// <summary>
        /// Finds a movie based upon specific search criteria
        /// </summary>
        /// <param name="query">Can search based upon Title, Year and Genre</param>
        /// <param name="cancellationToken">Can search based upon Title, Year and Genre</param>
        /// <returns>Http request</returns>
        /// <remarks>
        /// This search is an "and" search. If "Title" and "Year" are provided. It will narrow the selction to those that fulfill both critera. \
        /// For Instance, this search: \
        /// { \
        ///     Title: "super" \
        /// } \ 
        /// \
        /// \
        /// Will return 7 movies in the test data, whereas \
        /// \
        /// { \
        ///     Title: "super", \
        ///     Genre = "Romance", \
        ///     Year = 2004 \
        /// } \
        /// Will return 1 movie in the test data
        /// </remarks>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Output.Movie>>> Get([FromQuery] GetMoviesQuery query, CancellationToken cancellationToken)
        {
            var data = await _sender.Send(query, cancellationToken);
            return data == null || !data.Any() ? NotFound("Unable to find the data requested.") : Ok(data);
        }

        /// <summary>
        /// Get the top Rated movies, as judged by all reviewers
        /// </summary>
        /// <param name="numberOfMovies">How many of the top rated movie you want to see listed</param>
        /// <returns>An Http response</returns>
        [HttpGet]
        [Route("TopRankedMovies/{numberOfMovies}")]
        public ActionResult<List<Output.Movie>> TopRatedMovies(int numberOfMovies)
        {
            try
            {
                var results = _movieService.GetTopMovies(numberOfMovies);

                if (!results.Any())
                    return NotFound("Unable to find the top 5 movies");

                return Ok(results);
            }
            catch (Exception ex)
            {
                return ExceptionHandlingCode(ex);
            }
        }


        /// <summary>
        /// For any given reviewer, find the movies they gave the highest score to
        /// </summary>
        /// <param name="numberOfMovies">The number of top ranked films required</param>
        /// <param name="reviewerId">The Primary Key of the Reviewer in the database</param>
        /// <returns>An HTTP response</returns>
        [HttpGet]
        [Route("TopRankedMovies/{numberOfMovies}/Reviewer/{reviewerId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Output.Movie>> TopRankedMoviesByReviewer(int numberOfMovies, int reviewerId)
        {
            if (reviewerId == 0)
                return StatusCode(StatusCodes.Status400BadRequest, "A valid Id must be provided. 0 is not a valid Id");

            if (numberOfMovies == 0)
                return StatusCode(StatusCodes.Status400BadRequest, "A non zero number of movies");

            try
            {
                var results = _movieService.GetMoviesByReviewer(numberOfMovies, reviewerId);

                if (!results.Any())
                    return NotFound($"Unable to find top 5 reviewers for reviewer ID {reviewerId}");

                return Ok(results);
            }
            catch (Exception ex)
            {
                return ExceptionHandlingCode(ex);
            }
        }


        /// <summary>
        /// Add a new review or update an exsting review. 
        /// </summary>
        /// <param name="review">A Review Object that contains a movie review id, a reviewer id and a score </param>
        /// <returns>Http response</returns>
        [HttpPost]
        [Route("Review")]
        public ActionResult<List<Output.Movie>> AddReview(AddUpdateReview review)
        {
            if (!review.IsValidForSubmission())
            {
                return BadRequest($"Data provided  was : {review}");
            }

            var existingMovie = _movieService.GetMovieById(review.MovieId);
            if (null == existingMovie)
            {
                return NotFound($"Unable to find a movie with Id {review.MovieId}");
            }

            var existingReviewer = _movieService.GetReviewerById(review.ReviewerId);
            if (null == existingReviewer)
            {
                return NotFound($"Unable to find a reviewer with Id {review.ReviewerId}");
            }

            bool wasRecorded = _movieService.AddUpdateReview(review);
            if (!wasRecorded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to add or update the review");
            }

            return Ok("The review has been created / updated successfully");
        }


        /// <summary>
        /// This method is designed to be placed inside a catch block. 
        /// 
        /// It outputs the exception message into the error logs. A GUID is included to enable the exception to be found easily
        /// </summary>
        /// <param name="ex">The base exception thrown</param>
        /// <returns>Returns an HTTP 500 server exception error</returns>
        private ActionResult ExceptionHandlingCode(Exception ex)
        {
            Guid incidentNumber = Guid.NewGuid();

            _logger.LogError(incidentNumber.ToString() + ' ' + ex.Message);
            // return new ObjectResult(ex) { StatusCode = 500 };
            return Problem($"Problem retreving the data. Ask it to check log files for incidentNumber {incidentNumber}"
                          , null
                        , StatusCodes.Status500InternalServerError);
            //return StatusCode(StatusCodes.Status500InternalServerError,
            //                $"Problem retreving the data. Please check log files for incidentNumber {incidentNumber}");
        }
    }
}