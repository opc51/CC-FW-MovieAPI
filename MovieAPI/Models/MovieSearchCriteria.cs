namespace MovieAPI.Models
{
    /// <summary>
    /// The search criteria used to find movies
    /// </summary>
    public class MovieSearchCriteria
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
