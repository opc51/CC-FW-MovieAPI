using Movie.Repository.Entities.Common;
using Movie.Repository.Entities.Enum;

namespace Movie.Repository.SeedData
{
    public static class MovieData
    {
        public static List<Entities.Movie> Fetch()
        {
            var movies = new List<Entities.Movie>();

            var superHeroMovie = Entities.Movie.Create("Super Hero Movie ", ReleaseYear.Create(2004),
                   RunningTime.Create(180), GenreType.SuperHero);
            superHeroMovie.Id = 1;
            movies.Add(superHeroMovie);

            var superFunMovie = Entities.Movie.Create("Super Fun Movie ", ReleaseYear.Create(2002), RunningTime.Create(120), GenreType.Fun);
            superFunMovie.Id = 2;
            movies.Add(superFunMovie);

            var superFunMovieTwo = Entities.Movie.Create("Super Fun Movie 2", ReleaseYear.Create(2004), RunningTime.Create(180), GenreType.Fun);
            superFunMovieTwo.Id = 3;
            movies.Add(superFunMovieTwo);

            var superFunMovieThree = Entities.Movie.Create("Super Fun Movie 3", ReleaseYear.Create(2006), RunningTime.Create(90), GenreType.Fun);
            superFunMovieThree.Id = 4;
            movies.Add(superFunMovieThree);

            var superRomanceMovie = Entities.Movie.Create("Super Romance Movie", ReleaseYear.Create(2004), RunningTime.Create(120), GenreType.Romance);
            superRomanceMovie.Id = 5;
            movies.Add(superRomanceMovie);

            var superRomanceMovieTwo = Entities.Movie.Create("Super Romance Movie 2", ReleaseYear.Create(2006), RunningTime.Create(130), GenreType.Romance);
            superRomanceMovieTwo.Id= 6;
            movies.Add(superRomanceMovieTwo);

            var superHeroMovieTwo = Entities.Movie.Create("Super Hero Movie 2", ReleaseYear.Create(2011), RunningTime.Create(140), GenreType.Hero);
            superHeroMovieTwo.Id = 7;
            movies.Add(superHeroMovieTwo);

            var unknownMovie = Entities.Movie.Create("Unknown Title", ReleaseYear.Create(2011), RunningTime.Create(180), GenreType.Unknown);
            unknownMovie.Id = 8;
            movies.Add(unknownMovie);

            return movies;
        }
    }
}
