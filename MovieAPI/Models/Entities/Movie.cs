using MovieAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using MovieAPI.Models.Entities.Common;

namespace MovieAPI.Models.Entities
{
    /// <summary>
    /// A movie containing Id, Title, year of release and Genre
    /// </summary>
    [DebuggerDisplay("Title : {Title}, Genre : {Genre} ")]
    public class Movie
    {
        /// <summary>
        /// Private to prevent creation of an invalid movie
        /// </summary>
        private Movie() { }

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
        public ReleaseYear YearOfRelease { get; set; }


        /// <summary>
        /// The number of minutes that the movie was runs for
        /// </summary>
        [Required]
        public RunningTime RunningTime { get; set; }



        /// <summary>
        /// The "type" of movie .e.g. "Horror", "Superhero", etc 
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
        public static Movie Create(string title, ReleaseYear year, 
                                   RunningTime runningTime, GenreType genre)
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
