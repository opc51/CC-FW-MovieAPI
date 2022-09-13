﻿using FluentValidation;
using MediatR;
using Output = MovieAPI.Models.DTOs.Outputs;
using System.Collections.Generic;

namespace MovieAPI.Mediatr
{
    /// <summary>
    /// The query used by the GetMoviesHandler
    /// </summary>
    public class GetMoviesQuery : IRequest<List<Output.Movie>>
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
        /// The integer value of the specific Genre entered
        /// </summary>
        public int Genre { get; set; }
    }

    /// <summary>
    /// dsfdsdfd dffdf dfdfdff v
    /// </summary> 
    public class GetMoviesQueryValidator : AbstractValidator<GetMoviesQuery>
    {
        /// <summary>
        /// Restriction Put upon the movie query validator
        /// </summary>
        public GetMoviesQueryValidator()
        {
            RuleFor(c => c.Title).NotEmpty().When(c => c.Genre == 0 && c.Year == 0);
            RuleFor(c => c.Genre).NotEmpty().When(c => string.IsNullOrEmpty(c.Title) && c.Year == 0);
            RuleFor(c => c.Year).NotEmpty().When(c => string.IsNullOrEmpty(c.Title) && c.Genre == 0);
        }
    }
}
