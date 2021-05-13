using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

namespace MovieAPI.Repository
{
    public class APIContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Reviewer> Reviewers { get; set; }

        public APIContext(DbContextOptions options) : base(options)
        {
            LoadMovies();
            LoadReviewers();
            LoadReviews();
        }

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
                    , new Movie("Super Hero Movie ", 2004, 180, "Comedy")
                    , new Movie("Super Hero Movie 2 ", 2011, 180, "Comedy")
                    , new Movie("No one reviews me ", 2001, 180, "Unknown")
                );
                SaveChanges();
            }

        }

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
