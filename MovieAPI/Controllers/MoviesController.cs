using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieAPI.Interfaces;
using MovieAPI.Models;
using System;
using System.Linq;

namespace MovieAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IMovieService _movieService;

        public MoviesController(ILogger<MoviesController> logger, IMovieService movie)
        {
            _logger = logger;
            _movieService = movie;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] MovieSearchCriteria sc)
        {
            if (!sc.IsValid())
                return BadRequest($"Data provided  was : {sc}");
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


        private IActionResult ExceptionHandlingCode(Exception ex)
        {
            Guid incidentNumber = Guid.NewGuid();

            _logger.LogError(incidentNumber.ToString() + ' ' + ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError,
                            $"Problem retreving the data. Please check log files for incidentNumber {incidentNumber}");
        }
    }
}