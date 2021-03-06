using AutoFixture;
using AutoMapper;
using MovieAPI.Profiles;
using System;
using Xunit;
using DTO = MovieAPI.Models.DTOs;
using Entities = MovieAPI.Models.Entities;

namespace MovieTests
{
    public class AutomapperTests
    {
        private readonly IMapper _mapper;
        private readonly Fixture _fixture = new();

        public AutomapperTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MovieProfile>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void MovieEnity_convertsSuccesfully_toMovieDto()
        {
            //Arrange
            string movieName = _fixture.Create<string>();
            Entities.Movie movie = new(movieName, 2000, 180, "COMEDY");

            //Act
            DTO.Movie converted = _mapper.Map<Entities.Movie, DTO.Movie>(movie);

            //Assert
            Assert.True(string.Equals(movie.Title, converted.Title));
            Assert.Equal(movie.RunningTime, converted.RunningTime);
            Assert.Equal(DateTime.Now.Year - movie.YearOfRelease, converted.YearsPassedSinceOriginalRelease);
        }
    }
}
