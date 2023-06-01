using System.Diagnostics;

namespace Movie.Repository.Services.DTOs.Output
{
    /// <summary>
    /// Data Transfer Object used pass back Movie details
    /// </summary>
    [DebuggerDisplay("Title : {Title}, Genre : {Genre} ")]
    public record Movie
    {
        /// <summary>
        /// The title of the movie
        /// </summary>
        public string Title { get; init; }

        /// <summary>
        /// The number of years passed since the movie was originally released
        /// </summary>
        public int YearsPassedSinceOriginalRelease { get; init; }

        /// <summary>
        /// How many minutes did the movie run for
        /// </summary>
        public int RunningTime { get; init; }

        /// <summary>
        /// The type of movie
        /// </summary>
        public string Genre { get; init; }
    }
}
