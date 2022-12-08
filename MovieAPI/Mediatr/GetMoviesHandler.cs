using AutoMapper;
using MediatR;
using MovieAPI.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Output = MovieAPI.Models.DTOs.Outputs;

namespace MovieAPI.Mediatr
{
    /// <summary>
    /// The handler used to get a movie list from specific search criteria
    /// </summary>
    public class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery, List<Output.Movie>>
    {
        private readonly IMovieService _movieDataService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for the type <see cref="GetMoviesQueryHandler"/>
        /// </summary>
        /// <param name="movieDataService">The service used to get the data. Type of <see cref="IMovieService"/></param>
        /// <param name="mapper">The data mapper. Type of <see cref="IMapper"/></param>
        public GetMoviesQueryHandler(IMovieService movieDataService, IMapper mapper)
        {
            _movieDataService = movieDataService;
            _mapper = mapper;
        }

        /// <summary>
        /// Handler that accepts the search criteria and produces the data
        /// </summary>
        /// <param name="request">The search criteria. Type of <see cref="GetMoviesQuery"/></param>
        /// <param name="cancellationToken">The cancellation Token required for async operations. Type of <see cref="CancellationToken"/></param>
        /// <returns></returns>
        public async Task<List<Output.Movie>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            var movieEntities = await _movieDataService.GetMatchingMovies(request, cancellationToken);
            var data = _mapper.Map<List<Output.Movie>>(movieEntities);
            return data;
        }
    }
}
