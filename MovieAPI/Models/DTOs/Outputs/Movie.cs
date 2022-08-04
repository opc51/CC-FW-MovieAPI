using System.Diagnostics;

namespace MovieAPI.Models.DTOs.Outputs
{
    /// <summary>
    /// Data Transfer Object used pass back Movie details
    /// </summary>
    [DebuggerDisplay("Title : {Title}, Genre : {Genre} ")]
    public class Movie
    {
        /// <summary>
        /// The title of the movie
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The number of years passed since the movie was originally released
        /// </summary>
        public int YearsPassedSinceOriginalRelease { get; set; }

        /// <summary>
        /// How many minutes did the movie run for
        /// </summary>
        public int RunningTime { get; set; }

        /// <summary>
        /// The type of movie
        /// </summary>
        public string Genre { get; set; }
    }
}
