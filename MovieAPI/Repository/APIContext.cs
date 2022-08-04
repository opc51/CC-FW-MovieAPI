using Microsoft.EntityFrameworkCore;
using MovieAPI.Models.Entities;

namespace MovieAPI.Repository
{
    /// <summary>
    /// The API database context
    /// </summary>
    public class APIContext : DbContext
    {
        /// <summary>
        /// A Database set of Movie details
        /// </summary>
        public DbSet<Movie> Movies => Set<Movie>();


        /// <summary>
        /// A A database set of movie reviews
        /// </summary>
        public DbSet<Review> Reviews { get; set; }

        /// <summary>
        /// A database set of movies reviewers
        /// </summary>
        public DbSet<Reviewer> Reviewers { get; set; }


        /// <summary>
        /// A constructor for the API Datbase Context. 
        /// 
        /// Used to load movies, reviews and reviewers as well as set options in the base constructor
        /// </summary>
        /// <param name="options"></param>
        public APIContext(DbContextOptions options) : base(options)
        {
            LoadMovies();
            LoadReviewers();
            LoadReviews();
        }

        /// <summary>
        /// A public parameterless constructor needed for unit testing
        /// </summary>
        public APIContext()
        {

        }

        /// <summary>
        /// Pre populates a number of movies into the API Database context
        /// </summary>
        public void LoadMovies()
        {
            if (!Movies.AnyAsync().Result)
            {
                Movies.AddRange(
                    new Movie("Super Fun Movie", 2002, 120, "Comedy")
                    , new Movie("Super Fun Movie 2", 2004, 180, "Comedy")
                    , new Movie("Super Fun Movie 3", 2006, 90, "Comedy")
                    , new Movie("Super Romance Movie", 2004, 120, "Romance")
                    , new Movie("Super Romance Movie 2", 2006, 120, "Romance")
                    , new Movie("Super Hero Movie ", 2004, 180, "Hero")
                    , new Movie("Super Hero Movie 2 ", 2011, 180, "Hero")
                    , new Movie("No one reviews me ", 2011, 180, "Unknown")
                );
                SaveChanges();
            }
        }


        /// <summary>
        /// /// Pre populates a number of reviewers into the API Database context
        /// </summary>
        public void LoadReviewers()
        {
            if (!Reviewers.AnyAsync().Result)
            {
                Reviewers.AddRange(
                    new Reviewer("John")
                    , new Reviewer("Jane")
                    , new Reviewer("Josey")
                );
                SaveChanges();
            }
        }


        /// <summary>
        /// Pre populates a number of movies into the API Database context
        /// </summary>
        public void LoadReviews()
        {
            if (!Reviews.AnyAsync().Result)
            {
                Reviews.AddRange(
                    new Review(1, 1, 5), new Review(1, 2, 4), new Review(1, 3, 5), new Review(1, 4, 2), new Review(1, 5, 3), new Review(1, 6, 5)
                    , new Review(2, 1, 2), new Review(2, 2, 1), new Review(2, 3, 3), new Review(2, 4, 4), new Review(2, 5, 3), new Review(2, 6, 5)
                    , new Review(3, 1, 1), new Review(3, 2, 1), new Review(3, 3, 5), new Review(3, 4, 5), new Review(3, 5, 2), new Review(3, 6, 1)
                );
                SaveChanges();
            }
        }
    }
}
