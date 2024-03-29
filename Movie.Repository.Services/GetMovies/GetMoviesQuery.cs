﻿using FluentValidation;
using MediatR;
using Output = Movie.Repository.Services.DTOs.Output;

namespace Movie.Repository.Services
{
    /// <summary>
    /// The the input used by <see cref="GetMoviesHandler"/> 
    /// </summary>
    public class GetMoviesQuery : IRequest<List<Output.Movie>>
    {
        /// <summary>
        /// The name of the movie
        /// </summary>
        public string Title { get; set; } = string.Empty;


        /// <summary>
        /// The year the movie was released in the USA
        /// </summary>
        public int Year { get; set; }


        /// <summary>
        /// The integer value of the specific Genre entered
        /// </summary>
        public int Genre { get; set; }
    }

    /// <summary>
    /// Validates <see cref="GetMoviesQuery"/>
    /// </summary> 
    public class GetMoviesQueryValidator : AbstractValidator<GetMoviesQuery>
    {
        /// <summary>
        /// Restriction put upon the movie query validator
        /// </summary>
        public GetMoviesQueryValidator()
        {
            RuleFor(c => c.Title).NotEmpty().When(c => c.Genre == 0 && c.Year == 0);
            RuleFor(c => c.Genre).NotEmpty().When(c => string.IsNullOrEmpty(c.Title) && c.Year == 0);
            RuleFor(c => c.Year).NotEmpty().When(c => string.IsNullOrEmpty(c.Title) && c.Genre == 0);
        }
    }
}
