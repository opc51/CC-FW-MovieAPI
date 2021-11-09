namespace MovieAPI.Models
{
    /// <summary>
    /// DTO used to transfer lists of movies and ratings back to the caller
    /// </summary>
    public class MovieResultsList
    {
        /// <summary>
        /// The primary key of the movie
        /// </summary>
        public int MovieId { get; set; }
        
        /// <summary>
        /// The rating given for that movie
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// The name of the movie
        /// </summary>
        public string MovieTitle { get; set; }
        
        /// <summary>
        /// The year the movie was released in the USA
        /// </summary>
        public int YearOfRelease { get; set; }
        
        /// <summary>
        /// The length of time the movie runs for in minutes
        /// </summary>
        public int RunningTime { get; set; }


        /// <summary>
        /// The type of the movie. e.g. horror, romance, superhero
        /// </summary>
        public string Genres { get; set; }
    }
}


