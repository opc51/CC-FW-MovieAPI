using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieAPI.Interfaces;
using MovieAPI.Models;
using System;
using System.Linq;

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

        /// <summary>
        /// public constructor used to inject dependencies into the Movie Controller
        /// </summary>
        /// <param name="logger">The logging service that will be injected into to Movie Controller</param>
        /// <param name="movieDataService">The service class that will be injected to gather data from the API database context </param>
        public MoviesController(ILogger<MoviesController> logger, IMovieService movieDataService)
        {
            _logger = logger;
            _movieService = movieDataService;
        }

        /// <summary>
        /// Finds a movie based upon specific search criteria
        /// </summary>
        /// <param name="sc">Can search based upon Title, Year and Genre</param>
        /// <returns></returns>
        [HttpGet]
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
                    return NotFound($"No data found for search criteria {sc}");

                return Ok(results);
            }
            catch (Exception ex)
            {
                return ExceptionHandlingCode(ex);
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
                var results = _movieService.GetTopFiveMovies();

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
                return ExceptionHandlingCode(ex);
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
        private IActionResult ExceptionHandlingCode(Exception ex)
        {
            Guid incidentNumber = Guid.NewGuid();

            _logger.LogError(incidentNumber.ToString() + ' ' + ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                            $"Problem retreving the data. Please check log files for incidentNumber {incidentNumber}");
        }
    }
}