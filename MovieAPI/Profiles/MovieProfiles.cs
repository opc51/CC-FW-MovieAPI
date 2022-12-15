using AutoMapper;
using MovieAPI.Mediatr;
using MovieAPI.Models;
using Entity = MovieAPI.Models.Entities;
using Output = MovieAPI.Models.DTOs.Outputs;

namespace MovieAPI.Profiles
{
    /// <summary>
    /// AutoMapper Profile
    /// </summary>
    public class MovieProfiles : Profile
    {
        /// <summary>
        /// Constructor used to map Movie entities to Movie DTO's
        /// </summary>
        public MovieProfiles()
        {
            CreateMap<Entity.Movie, Output.Movie>()
                .ForMember(
                    dto => dto.YearsPassedSinceOriginalRelease,
                    ent => ent.MapFrom(src => src.YearOfRelease.NumberOfYearsAgoReleased())
                );

            CreateMap<MovieSearchCriteria, GetMoviesQuery>()
                .ForMember(q => q.Genre,
                            sc => sc.MapFrom(sc => sc.GenreAsInteger)
                 );
        }
    }
}
