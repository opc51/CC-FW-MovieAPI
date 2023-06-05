using Movie.Domain.Enum;

namespace Movie.Repository.SeedData
{
    public static class MovieData
    {
        public static List<Domain.Movie> Fetch()
        {
            var movies = new List<Domain.Movie>();

            var superHeroMovie = Domain.Movie.Create("Super Hero Movie ", Domain.ReleaseYear.Create(2004),
                   Domain.RunningTime.Create(180), GenreType.SuperHero);
            superHeroMovie.Id = 1;
            movies.Add(superHeroMovie);

            var superFunMovie = Domain.Movie.Create("Super Fun Movie ", Domain.ReleaseYear.Create(2002), Domain.RunningTime.Create(120), GenreType.Fun);
            superFunMovie.Id = 2;
            movies.Add(superFunMovie);

            var superFunMovieTwo = Domain.Movie.Create("Super Fun Movie 2", Domain.ReleaseYear.Create(2004), Domain.RunningTime.Create(180), GenreType.Fun);
            superFunMovieTwo.Id = 3;
            movies.Add(superFunMovieTwo);

            var superFunMovieThree = Domain.Movie.Create("Super Fun Movie 3", Domain.ReleaseYear.Create(2006), Domain.RunningTime.Create(90), GenreType.Fun);
            superFunMovieThree.Id = 4;
            movies.Add(superFunMovieThree);

            var superRomanceMovie = Domain.Movie.Create("Super Romance Movie", Domain.ReleaseYear.Create(2004), Domain.RunningTime.Create(120), GenreType.Romance);
            superRomanceMovie.Id = 5;
            movies.Add(superRomanceMovie);

            var superRomanceMovieTwo = Domain.Movie.Create("Super Romance Movie 2", Domain.ReleaseYear.Create(2006), Domain.RunningTime.Create(130), GenreType.Romance);
            superRomanceMovieTwo.Id= 6;
            movies.Add(superRomanceMovieTwo);

            var superHeroMovieTwo = Domain.Movie.Create("Super Hero Movie 2", Domain.ReleaseYear.Create(2011), Domain.RunningTime.Create(140), GenreType.Hero);
            superHeroMovieTwo.Id = 7;
            movies.Add(superHeroMovieTwo);

            var unknownMovie = Domain.Movie.Create("Unknown Title", Domain.ReleaseYear.Create(2011), Domain.RunningTime.Create(180), GenreType.Unknown);
            unknownMovie.Id = 8;
            movies.Add(unknownMovie);

            return movies;
        }
    }
}
