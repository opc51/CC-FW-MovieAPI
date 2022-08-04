using AutoMapper;
using Entity = MovieAPI.Models.Entities;
using Output = MovieAPI.Models.DTOs.Outputs;
using System;

namespace MovieAPI.Profiles
{
    /// <summary>
    /// AutoMapper Profile
    /// </summary>
    public class MovieProfile : Profile
    {
        /// <summary>
        /// Constructor used to map Movie entities to Movie DTO's
        /// </summary>
        public MovieProfile()
        {
            CreateMap<Entity.Movie, Output.Movie>()
                .ForMember(
                    dto => dto.YearsPassedSinceOriginalRelease,
                    ent => ent.MapFrom(ent => DateTime.Now.Year - ent.YearOfRelease)
                );
        }
    }
}
