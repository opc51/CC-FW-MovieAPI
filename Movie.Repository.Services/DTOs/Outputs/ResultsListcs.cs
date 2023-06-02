using System.Diagnostics;

namespace Movie.Repository.Services.DTOs.Output
{
    /// <summary>
    /// DTO used to transfer lists of movies and ratings back to the caller
    /// </summary>
    [DebuggerDisplay("Title : {MovieTitle}, Rating : {Rating}")]
    public record MovieResult
    {
        /// <summary>
        /// The primary key of the movie
        /// </summary>
        public int MovieId { get; init; }

        /// <summary>
        /// The rating given for that movie
        /// </summary>
        public double Rating { get; init; }

        /// <summary>
        /// The name of the movie
        /// </summary>
        public string MovieTitle { get; init; } = string.Empty;

        /// <summary>
        /// The year the movie was released in the USA
        /// </summary>
        public int YearOfRelease { get; init; }

        /// <summary>
        /// The length of time the movie runs for in minutes
        /// </summary>
        public int RunningTime { get; init; }


        /// <summary>
        /// The type of the movie. e.g. horror, romance, superhero
        /// </summary>
        public string Genres { get; init; } = string.Empty;
    }
}