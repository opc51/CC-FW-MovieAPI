using FluentValidation;
using MediatR;
using Movie.Repository.Services.DTOs.Output;

namespace Movie.Repository.Services.TopRatedMovies
{
    public class GetTopRatedMoviesQuery : IRequest<List<MovieResult>>
    {
        /// <summary>
        /// The number of top rated movies to retrieve
        /// </summary>
        public int NumberOfMovies { get; set; }
    }

    public class GetTopRatedMoviesQueryValidator : AbstractValidator<GetTopRatedMoviesQuery>
    {
        public GetTopRatedMoviesQueryValidator()
        {
            RuleFor(q => q.NumberOfMovies).GreaterThan(0);
        }
    }
}
