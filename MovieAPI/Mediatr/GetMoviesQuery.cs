using FluentValidation;
using MediatR;
using MovieAPI.Models.DTOs.Outputs;
using System.Collections.Generic;

namespace MovieAPI.Mediatr
{
    /// <summary>
    /// The query used by the GetMoviesHandler
    /// </summary>
    public class GetMoviesQuery : IRequest<List<MovieResultsList>>
    {
        /// <summary>
        /// The name of the movie
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// The year the movie was released in the USA
        /// </summary>
        public int Year { get; set; }


        /// <summary>
        /// The type of Movie .e.g. "Horror", "Sci Fi", ""
        /// </summary>
        public string Genre { get; set; }
    }

    /// <summary>
    /// dsfdsdfd dffdf dfdfdff v
    /// </summary> 
    public class GetMoviesQueryValidator : AbstractValidator<GetMoviesQuery>
    {
        /// <summary>
        /// dsd dsd sdds
        /// </summary>
        public GetMoviesQueryValidator()
        {
            RuleFor(c => c.Title).NotEmpty().When(c => string.IsNullOrEmpty(c.Genre) || c.Year == 0);
            RuleFor(c => c.Genre).NotEmpty().When(c => string.IsNullOrEmpty(c.Title) || c.Year == 0);
            RuleFor(c => c.Year).NotEmpty().When(c => string.IsNullOrEmpty(c.Title) || string.IsNullOrEmpty(c.Genre));
        }
    }
}
