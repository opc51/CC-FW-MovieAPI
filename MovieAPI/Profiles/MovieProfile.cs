using AutoMapper;
using Entity = MovieAPI.Models.Entities;
using DTO = MovieAPI.Models.DTOs;
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
            CreateMap<Entity.Movie, DTO.Movie>()
                .ForMember(
                    dto => dto.YearsPassedSinceOriginalRelease,
                    ent => ent.MapFrom(ent => DateTime.Now.Year - ent.YearOfRelease)
                );
        }
    }
}
