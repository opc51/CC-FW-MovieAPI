using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movie.Repository.Services;
using Movie.Repository.Services.TopRankedMoviesByReviewer;
using Movie.Repository.Services.TopRatedMovies;
using Movie.Respository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
        /// <param name="query">Type of <see cref="GetTopRatedMoviesQuery"/></param>
        /// <param name="cancellationToken"></param>
        /// <returns>An Http response</returns>
        [HttpGet]
        [Route("TopRanked")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Output.MovieResult>>> TopRatedMovies([FromQuery] GetTopRatedMoviesQuery query, CancellationToken cancellationToken)
        {
            var data = await _sender.Send(query, cancellationToken);
            return data == null || !data.Any() ? NotFound("Unable to find the data requested.") : Ok(data);
        }


        /// <summary>
        /// For any given reviewer, find the movies they gave the highest score to
        /// </summary>
        /// <returns>An HTTP response</returns>
        [HttpGet]
        [Route("TopRanked/Reviewer/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Output.Movie>>> TopRankedMoviesByReviewer([FromQuery] TopRankedMoviesByReviewerQuery query, CancellationToken cancellationToken) 
        {
            var data = await _sender.Send(query, cancellationToken);
            return data == null || !data.Any() ? NotFound("Unable to find the data requested.") : Ok(data);
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
    }
}