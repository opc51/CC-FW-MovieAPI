using AutoMapper;
using Movie.Repository.Services;
using Entity = Movie.Domain;
using Output = Movie.Repository.Services.DTOs.Output;

namespace Movie.API.AutoMapper
{
    /// <summary>
    /// AutoMapper profile for conversion of <see cref="Entity.Movie"/>, 
    /// <see cref="Output.Movie"/> and <see cref="MovieSearchCriteria"/>
    /// </summary>
    public class MovieProfiles : Profile
    {
        /// <summary>
        /// Constructor used to instantiate an instance of <see cref="MovieProfiles"/>
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
