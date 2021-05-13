Not Implemented

Not using Async methods. As this project uses an InMemory Database rather than an external data source there is no need for
async methods

Refactor
Use enum for Genres? - using string- means adding more without needing to release code base again

MOQ for service class, It is possible but will take a bit for effort 
https://www.andrewhoefling.com/Blog/Post/moq-entity-framework-dbset


MOQ Verify that the loggin entries are being created

Add the final controller actions, add the updaing of the code.



















From GetTopFiveMovies
            /*  working code (ish)
            var moviesWithScores =
                from reviews in _data.Reviews
                join movies in _data.Movies on reviews.MovieId equals movies.Id
                select new { MovieId = reviews.MovieId, MovieTitle = movies.Title, Rating = reviews.Score};

            var grouped  = moviesWithScores.ToList().GroupBy(x => x.MovieId).ToList();


            List<ResultList> finalScores = new();
            foreach (var movie in grouped.ToList())
            {
                finalScores.Add(new ResultList()
                {
                    MovieId = movie.Key,
                    Rating = movie.Average(x => x.Rating),
                    MovieTitle = movie.Select(x => x.MovieTitle).First()
                });
            }

            IQueryable<ResultList> abc = finalScores.AsQueryable().OrderByDescending(x => x.Rating).ThenBy(x => x.MovieTitle);
            var sdf = abc.ToList();
            */


