using MediatR;
using MovieAPI.Interfaces;
using Output = MovieAPI.Models.DTOs.Outputs;
using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using System.Threading.Tasks;

namespace MovieAPI.Mediatr
{
    /// <summary>
    /// The handler used to get a movie list from specific search criteria
    /// </summary>
    public class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery, List<Output.Movie>>
    {
        private readonly IMovieService _movieDataService;
        private readonly IMapper _mapper;

        public GetMoviesQueryHandler(IMovieService movieDataService, IMapper mapper)
        {
            _movieDataService = movieDataService;
            _mapper = mapper;
        }
        /// <summary>
        /// The main handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Output.Movie>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            var movieEntities = await _movieDataService.GetMatchingMovies(request, cancellationToken);
            var data = _mapper.Map<List<Output.Movie>>(movieEntities);
            return data;
        }

    }
}
