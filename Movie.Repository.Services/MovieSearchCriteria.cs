﻿using Movie.Domain.Enum;

namespace Movie.Repository.Services
{
    /// <summary>
    /// The search criteria used to find movies - // to do - move this
    /// </summary>
    public record MovieSearchCriteria
    {
        /// <summary>
        /// The name of the movie
        /// </summary>
        public string Title { get; init; } = string.Empty;


        /// <summary>
        /// The year the movie was released in the USA
        /// </summary>
        public int Year { get; init; }


        /// <summary>
        /// The type of Movie .e.g. "SuperHero", "Comedy", "Romance"
        /// </summary>
        public string Genre { get; init; } = string.Empty;


        /// <summary>
        /// Used by Entity framework to 
        /// </summary>
        public int GenreAsInteger
        {
            get
            {
                if (GenreType.TryFromName(Genre, out var genre))
                {
                    return genre.Value;
                }
                return 0;
            }
        }


        /// <summary>
        /// Checks that the search criteria contains some valid search criteria
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Title) || Year != 0 || !string.IsNullOrWhiteSpace(Genre);
        }


        /// <summary>
        /// overide of the ToString method that contains the Title, the year and the Genre
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Title : {Title}, Year : {Year}, Genre {Genre}";
        }
    }
}