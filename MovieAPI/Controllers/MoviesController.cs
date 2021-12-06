using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieAPI.Interfaces;
using MovieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO = MovieAPI.Models.DTOs;

namespace MovieAPI.Controllers
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

        /// <summary>
        /// public constructor used to inject dependencies into the Movie Controller
        /// </summary>
        /// <param name="logger">The logging service that will be injected into to Movie Controller</param>
        /// <param name="movieDataService">The service class that will be injected to gather data from the API database context </param>
        /// <param name="mapper">Automapper instance to convert databse movies to dto movies </param> 
        public MoviesController(ILogger<MoviesController> logger, IMovieService movieDataService, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _movieService = movieDataService ?? throw new ArgumentNullException(nameof(movieDataService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Finds a movie based upon specific search criteria
        /// </summary>
        /// <param name="sc">Can search based upon Title, Year and Genre</param>
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get([FromQuery] MovieSearchCriteria sc)
        {
            if (!sc.IsValid())
            {
                _logger.LogError("Bad request was recieved");
                return BadRequest($"Data provided  was : {sc}");
            }
            try
            {
                var results = _movieService.GetMatchingMovies(sc);

                if (!results.Any())
                    return NotFound(new ProblemDetails() { Detail = "No data found for search criteria {sc}" });

                return Ok(_mapper.Map<List<DTO.Movie>>(results));
            }
            catch (Exception ex)
            {
                //return Problem()
                //ProblemDetails() { Status = 500, Detail = ex.Message };

                return ExceptionHandlingCode<List<DTO.Movie>>(ex);
            }
        }

        /// <summary>
        /// Get the 5 movies that have been given the highest rating by all reviewers
        /// </summary>
        /// <returns>An Http response</returns>
        [HttpGet]
        [Route("Top5")]
        public IActionResult TopFiveByAllRatings()
        {
            try
            {
                var results = _movieService.GetTopMovies(5);

                if (!results.Any())
                    return NotFound("Unable to find the top 5 movies");

                return Ok(results);
            }
            catch (Exception ex)
            {
                return ExceptionHandlingCode<DTO.MovieResultsList>(ex);
            }
        }


        /// <summary>
        /// For any given reviewer, find the 5 movies they gave the highest score to
        /// </summary>
        /// <param name="reviewerId">The Primary Key of the Reviewer in the database</param>
        /// <returns>An HTTP response</returns>
        [HttpGet]
        [Route("Top5ByReviewer/{reviewerId}")]
        public IActionResult TopFiveMoviesByReviewer(int reviewerId)
        {
            if (reviewerId == 0)
                return StatusCode(StatusCodes.Status400BadRequest, "A valid Id must be provided. 0 is not a valid Id");

            try
            {
                var results = _movieService.GetTopFiveMoviesByReviewer(reviewerId);

                if (!results.Any())
                    return NotFound($"Unable to find top 5 reviewers for reviewer ID {reviewerId}");

                return Ok(results);
            }
            catch (Exception ex)
            {
                return ExceptionHandlingCode<DTO.MovieResultsList>(ex);
            }
        }


        /// <summary>
        /// Add a new review or update an exsting review. 
        /// </summary>
        /// <param name="review">A Review Object that contains a movie review id, a reviewer id and a score </param>
        /// <returns>Http response</returns>
        [HttpPost]
        [Route("AddReview")]
        public IActionResult AddReview(AddUpdateReview review)
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
        private ActionResult ExceptionHandlingCode<T>(Exception ex) where T : class
        {
            Guid incidentNumber = Guid.NewGuid();

            _logger.LogError(incidentNumber.ToString() + ' ' + ex.Message);

            //return Problem($"Problem retreving the data. Please check log files for incidentNumber {incidentNumber}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                            $"Problem retreving the data. Please check log files for incidentNumber {incidentNumber}");
        }
    }
}