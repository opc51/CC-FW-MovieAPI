using MediatR;
using MovieAPI.Interfaces;
using MovieAPI.Models.DTOs.Outputs;
using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using System.Threading.Tasks;

namespace MovieAPI.Mediatr
{
    /// <summary>
    /// The handler used to get a movie list from specific search criteria
    /// </summary>
    public class GetMoviesHandler : IRequestHandler<GetMoviesQuery, List<MovieResultsList>>
    {
        private readonly IMovieService _movieDataService;
        private readonly IMapper _mapper;

        public GetMoviesHandler(IMovieService movieDataService)
        {
            _movieDataService = movieDataService;
        }
        /// <summary>
        /// The main handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<List<MovieResultsList>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            var movieEntities = await _movieDataService.GetMatchingMovies(request, cancellationToken);
            var data =  _mapper.Map<List<MovieResultsList>>(movieEntities);
            return data;
        }

    }
}
