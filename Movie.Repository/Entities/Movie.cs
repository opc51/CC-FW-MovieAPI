using Movie.Repository.Entities.Common;
using Movie.Repository.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Movie.Repository.Entities
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
        private Movie()
        {
            reviews = new List<Review>();
        }

        /// <summary>
        /// The primary key of the Movie
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the movie
        /// </summary>
        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The year the movie was release in the USA
        /// </summary>
        [Required]
        public ReleaseYear YearOfRelease { get; set; }

        /// <summary>
        /// The number of minutes that the movie was runs for
        /// </summary>
        [Required]
        public RunningTime RunningTime { get; set; } = null!;

        /// <summary>
        /// The "type" of movie .e.g. "Horror", "Superhero", etc 
        /// </summary>
        public GenreType Genre { get; set; }

        private List<Review> reviews { get; set; }

        /// <summary>
        /// Movie reviews of type <see cref="Review"/> attached to this Movie
        /// </summary>
        public IReadOnlyCollection<Review> Reviews { get { return reviews; } }

        /// <summary>
        /// Contains the average score of the movie has over all 
        /// </summary>
        public double GetAverageScore
        {
            get
            {
                if (!Reviews.Any())
                {
                    return 0;
                }
                return reviews.Average(r => r.Score);
            }

            private set { }
        }

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
            return new Movie()
            {
                Id = new Random().Next(),
                Title = title,
                YearOfRelease = year,
                RunningTime = runningTime,
                Genre = genre
            };
        }

        /// <summary>
        /// Used to add reviews of type <see cref="Review"/> to <see cref="Movie"/>.
        /// 
        /// New reviews are appended to old reviews
        /// </summary>
        /// <param name="reviewList">An <see cref="Enumerable"/> of <see cref="Review"/></param>
        /// <exception cref="System.ArgumentException">If no reviews are passed in</exception>
        public void AddReviews(IEnumerable<Review> reviewList)
        {
            if (!reviewList.Any())
            {
                throw new System.ArgumentException("Reviews cannot be null or empty when adding to an Movie");
            }
            reviews.AddRange(reviewList);
        }
    }
}
