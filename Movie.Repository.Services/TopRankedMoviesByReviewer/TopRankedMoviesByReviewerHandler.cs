using MediatR;
using Movie.Domain;
using Movie.Repository.Services.DTOs.Output;
using Movie.Repository.Services.TopRatedMovies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Movie.Repository.Services.TopRankedMoviesByReviewer
{
    public class TopRankedMoviesByReviewerHandler : IRequestHandler<TopRankedMoviesByReviewerQuery, List<MovieResult>>
    {
        private readonly IMovieService _movieService;

        public TopRankedMoviesByReviewerHandler(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<List<MovieResult>> Handle(TopRankedMoviesByReviewerQuery request, CancellationToken cancellationToken)
        {
            return await _movieService.GetMoviesByReviewer(request, cancellationToken);
        }
    }
}