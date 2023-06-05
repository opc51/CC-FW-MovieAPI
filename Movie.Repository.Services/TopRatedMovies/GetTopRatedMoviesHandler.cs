using System;
using AutoMapper;
using MediatR;
using Movie.Repository.Services.DTOs.Output;
using Output = Movie.Repository.Services.DTOs.Output;

namespace Movie.Repository.Services.TopRatedMovies
{
    public class GetTopRatedMoviesHandler : IRequestHandler<GetTopRatedMoviesQuery, List<Output.MovieResult>>
    {
        private readonly IMovieService _movieDataService;

        public GetTopRatedMoviesHandler(IMovieService movieDataService)
        {
            _movieDataService = movieDataService;
        }

        public async Task<List<Output.MovieResult>> Handle(GetTopRatedMoviesQuery request, CancellationToken cancellationToken)
        {
            return await _movieDataService.GetTopMovies(request, cancellationToken);
        }
    }
}
