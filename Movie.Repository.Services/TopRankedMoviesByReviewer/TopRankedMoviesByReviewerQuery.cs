using FluentValidation;
using MediatR;
using Movie.Repository.Services.DTOs.Output;

namespace Movie.Repository.Services.TopRankedMoviesByReviewer
{
    public class TopRankedMoviesByReviewerQuery : IRequest<List<MovieResult>>
    {
        /// <summary>
        /// The number of movies to fetch
        /// </summary>
        public int NumberOfMovies { get; set; }

        /// <summary>
        /// The <see cref="int"/> id of the <see cref="Domain.Reviewer"/>
        /// </summary>
        public int ReviewerId { get; set; }
    }

    public class TopRankedMoviesByReviewerQueryValidator : AbstractValidator<TopRankedMoviesByReviewerQuery>
    {
        public TopRankedMoviesByReviewerQueryValidator()
        {
            RuleFor(x => x.NumberOfMovies).NotEmpty().GreaterThan(0);
            RuleFor(x => x.ReviewerId).NotEmpty().GreaterThan(0);
        }
    }
}
