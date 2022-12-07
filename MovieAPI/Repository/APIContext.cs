using Microsoft.EntityFrameworkCore;
using MovieAPI.Models.Entities;
using MovieAPI.Models.Entities.Common;
using MovieAPI.Models.Enum;

namespace MovieAPI.Repository
{
    /// <summary>
    /// The API database context
    /// </summary>
    public class APIContext : DbContext
    {
        /// <summary>
        /// Needed to handle smart enums
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>()
                        .Property(m => m.Genre)
                        .HasConversion(m => m.Value, m => GenreType.FromValue(m));

            modelBuilder.Entity<Movie>()
                .Property(m => m.RunningTime)
                .HasConversion(v => v.Value, v => RunningTime.Create(v));

            modelBuilder.Entity<Movie>()
                .Property(m => m.YearOfRelease)
                .HasConversion(v => v.Value, v => ReleaseYear.Create(v));
        }

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
                    Movie.Create("Super Hero Movie ", ReleaseYear.Create(2004),
                                   RunningTime.Create(180), GenreType.SuperHero)
                    , Movie.Create("Super Fun Movie", ReleaseYear.Create(2002),
                                    RunningTime.Create(120), GenreType.Comedy)
                    , Movie.Create("Super Fun Movie 2", ReleaseYear.Create(2004),
                                    RunningTime.Create(180), GenreType.Comedy)
                    , Movie.Create("Super Fun Movie 3", ReleaseYear.Create(2006),
                                    RunningTime.Create(90), GenreType.Comedy)
                    , Movie.Create("Super Romance Movie", ReleaseYear.Create(2004),
                                    RunningTime.Create(120), GenreType.Romance)
                    , Movie.Create("Super Romance Movie 2", ReleaseYear.Create(2006),
                                    RunningTime.Create(120), GenreType.Romance)
                    , Movie.Create("Super Hero Movie 2 ", ReleaseYear.Create(2011),
                                    RunningTime.Create(180), GenreType.SuperHero)
                    , Movie.Create("No one reviews me ", ReleaseYear.Create(2011),
                                    RunningTime.Create(180), GenreType.Unknown)
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
