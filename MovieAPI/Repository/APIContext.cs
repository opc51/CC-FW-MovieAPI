using Microsoft.EntityFrameworkCore;
using MovieAPI.Models.Domain;
using MovieAPI.Models.Domain.Common;
using MovieAPI.Models.Enum;
using System.Reflection;

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
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// A Database set of Movie details
        /// </summary>
        public DbSet<Movie> Movies => Set<Movie>();


        /// <summary>
        /// A database set of movie reviews
        /// </summary>
        public DbSet<Review> Reviews => Set<Review>();

        /// <summary>
        /// A database set of movies reviewers
        /// </summary>
        public DbSet<Reviewer> Reviewers => Set<Reviewer>();


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
        /// Pre populates a number of movies into the API Database context
        /// </summary>
        public void LoadMovies()
        {
            if (!Movies.AnyAsync().Result)
            {
                Movies.AddRange(
                    Movie.Create("Super Hero Movie ", ReleaseYear.Create(2004), RunningTime.Create(180), GenreType.SuperHero),
                    Movie.Create("Super Fun Movie ", ReleaseYear.Create(2002), RunningTime.Create(120), GenreType.Fun),
                    Movie.Create("Super Fun Movie 2", ReleaseYear.Create(2004), RunningTime.Create(180), GenreType.Fun),
                    Movie.Create("Super Fun Movie 3", ReleaseYear.Create(2006), RunningTime.Create(90), GenreType.Fun),
                    Movie.Create("Super Romance Movie", ReleaseYear.Create(2004), RunningTime.Create(120), GenreType.Romance),
                    Movie.Create("Super Romance Movie 2", ReleaseYear.Create(2006), RunningTime.Create(130), GenreType.Romance),
                    Movie.Create("Super Hero Movie 2", ReleaseYear.Create(2011), RunningTime.Create(140), GenreType.Hero),
                    Movie.Create("Unknown Title", ReleaseYear.Create(2011), RunningTime.Create(180), GenreType.Unknown)
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
                    Reviewer.Create("JohnTheBrit", "john@john.com", "gb", "01234875456")
                    , Reviewer.Create("JaneAmerican", "john@john.com", "US", "3333334444")
                    , Reviewer.Create("JoseyFrance", "john@john.com", "Fr", "123456789")
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
                    Review.Create(1, 1, 5), Review.Create(1, 2, 4), Review.Create(1, 3, 5), Review.Create(1, 4, 2), Review.Create(1, 5, 3), Review.Create(1, 6, 5)
                    , Review.Create(2, 1, 2), Review.Create(2, 2, 1), Review.Create(2, 3, 3), Review.Create(2, 4, 4), Review.Create(2, 5, 3), Review.Create(2, 6, 5)
                    , Review.Create(3, 1, 1), Review.Create(3, 2, 1), Review.Create(3, 3, 5), Review.Create(3, 4, 5), Review.Create(3, 5, 2), Review.Create(3, 6, 1)
                );
                SaveChanges();
            }
        }
    }
}


//var reviewerList = new List<Reviewer>();
//reviewerList.AddRange(new List<Reviewer>()
//{
//    Reviewer.Create("JohnTheBrit", "john@john.com", "gb", "01234875456"),
//    Reviewer.Create("JaneAmerican", "john@john.com", "US", "3333334444"),
//    Reviewer.Create("JoseyFrance", "john@john.com", "Fr", "123456789")
//});

//var reviewList = new List<Review>();
//Review.AddRange(new List<Review>() {
//    Review.Create(1, 1, 5), Review.Create(2, 1, 2), Review.Create(3, 1, 1),
//    Review.Create(1, 2, 4), Review.Create(2, 2, 1), Review.Create(3, 2, 1),
//    Review.Create(1, 3, 5), Review.Create(2, 3, 3), Review.Create(3, 3, 5),
//    Review.Create(1, 4, 2), Review.Create(2, 4, 4), Review.Create(3, 4, 5),
//    Review.Create(1, 5, 3), Review.Create(2, 5, 3), Review.Create(3, 5, 2),
//    Review.Create(1, 6, 5), Review.Create(2, 6, 5), Review.Create(3, 6, 1)
//});

//    var movieList = new List<Movie>();
//    movieList.AddRange(new List<Movie>() {
//        Movie.Create("Super Hero Movie ", ReleaseYear.Create(2004), RunningTime.Create(180), GenreType.SuperHero),
//        Movie.Create("Super Fun Movie ", ReleaseYear.Create(2002), RunningTime.Create(120), GenreType.Fun),
//        Movie.Create("Super Fun Movie 2", ReleaseYear.Create(2004), RunningTime.Create(180), GenreType.Fun),
//        Movie.Create("Super Fun Movie 3", ReleaseYear.Create(2006), RunningTime.Create(90), GenreType.Fun),
//        Movie.Create("Super Romance Movie", ReleaseYear.Create(2004), RunningTime.Create(120), GenreType.Romance),
//        Movie.Create("Super Romance Movie 2", ReleaseYear.Create(2006), RunningTime.Create(130), GenreType.Romance),
//        Movie.Create("Super Hero Movie 2", ReleaseYear.Create(2011), RunningTime.Create(140), GenreType.Hero),
//        Movie.Create("Unknown Title", ReleaseYear.Create(2011), RunningTime.Create(180), GenreType.Unknown)
//});


//modelBuilder.Entity<Reviewer>()
//    .Property(x => x.Id)
//    .ValueGeneratedOnAdd()
//    .HasValueGenerator<ReviewerIdValueGenerator>();

//modelBuilder.Entity<Review>()
//    .Property(x => x.Id)
//    .ValueGeneratedOnAdd()
//    .HasValueGenerator<ReviewIdValueGenerator>();

//modelBuilder.Entity<Movie>()
//    .Property(x => x.Id)
//    .ValueGeneratedOnAdd()
//    .HasValueGenerator<MovieIdValueGenerator>();

//modelBuilder.Entity<Reviewer>()
//    .HasData(reviewerList);

//modelBuilder.Entity<Review>()
//    .HasData(reviewList);

//modelBuilder.Entity<Movie>()
//    .HasData(Movie.Create("Super ero Movie ", ReleaseYear.Create(2004), RunningTime.Create(180), GenreType.SuperHero));