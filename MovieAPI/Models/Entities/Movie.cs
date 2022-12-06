using MovieAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MovieAPI.Models.Entities
{
    /// <summary>
    /// A movie containing Id, Title, year of release and Genre
    /// </summary>
    [DebuggerDisplay("Title : {Title}, Genre : {Genre} ")]
    public class Movie
    {
        /// <summary>
        /// A movie that contains a title, a year of release, a running time and a genre.
        /// </summary>
        private Movie()
        {
            // allows an invalid state
            // configuration
            // able to create domain entity is a valid cooncerns
            // private enmpty constructor
        }

        /// <summary>
        /// Constructor for a movie
        /// </summary>
        /// <param name="title">The name of the movie, string</param>
        /// <param name="yearOfRelease">The year of release, string</param>
        /// <param name="runnigTime"> The length of the movie measure in minutes, int</param>
        /// <param name="genre">The type of movie .e.g. "horror", "romance". string</param>
        public Movie(string title, int yearOfRelease, int runnigTime, GenreType genre)
        {
            Title = title;
            YearOfRelease = yearOfRelease;
            RunningTime = runnigTime;
            Genre = genre;
        }

        /// <summary>
        /// The primary key of the Movie
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// The name of the movie
        /// </summary>
        [Required]
        public string Title { get; set; }



        /// <summary>
        /// The year the movie was release in the USA
        /// </summary>
        [Required]
        public int YearOfRelease { get; set; }


        /// <summary>
        /// The number of minutes that the movie was runs for
        /// </summary>
        [Required]
        public int RunningTime { get; set; }



        /// <summary>
        /// The "type" of moviefor example "Horror", "Superhero", etc 
        /// </summary>
        public GenreType Genre { get; set; }

        /// <summary>
        /// Used to create a new instance of type <see cref="Movie"/>
        /// </summary>
        /// <param name="title">The name of the Movie. Type <see cref="string"/> </param>
        /// <param name="year">The Year the movie was released. Type <see cref="int"/></param>
        /// <param name="runningTime">The Year the movie was released. Type <see cref="int"/></param>
        /// <param name="genre">The Year the movie was released. Type <see cref="GenreType"/></param>
        /// <returns></returns>
        public static Movie Create(string title, int year, int runningTime, GenreType genre)
        {
            return new Movie() { 
                Title = title,
                YearOfRelease = year,
                RunningTime = runningTime,
                Genre = genre
            };
        }
    }
}
