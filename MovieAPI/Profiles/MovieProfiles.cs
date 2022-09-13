using AutoMapper;
using Entity = MovieAPI.Models.Entities;
using Output = MovieAPI.Models.DTOs.Outputs;
using System;
using MovieAPI.Models;
using MovieAPI.Mediatr;

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
                    ent => ent.MapFrom(ent => DateTime.Now.Year - ent.YearOfRelease)
                );
            CreateMap<MovieSearchCriteria, GetMoviesQuery>()
                .ForMember(q => q.Genre,
                            sc => sc.MapFrom(sc => sc.GenreAsInteger)
                 );
        }
    }
}
