using AutoMapper;
using Movie.API.Models;
using Movie.Repository.Services;
using Entity = Movie.Repository.Entities;
using Output = Movie.Repository.Services.DTOs.Output;

namespace Movie.API.AutoMapper
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
