using AutoFixture;
using AutoMapper;
using MovieAPI.Models.DTOs.Outputs;
using MovieAPI.Models.Entities.Common;
using MovieAPI.Models.Enum;
using MovieAPI.Profiles;
using System;
using Xunit;
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
                cfg.AddProfile<MovieProfiles>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void MovieEnity_convertsSuccesfully_toMovieDto()
        {
            //Arrange
            string movieName = _fixture.Create<string>();
            var movie = Entities.Movie.Create(movieName, ReleaseYear.Create(2000),
                                              RunningTime.Create(180), GenreType.Comedy);

            //Act
            Movie converted = _mapper.Map<Entities.Movie, Movie>(movie);

            //Assert
            Assert.True(string.Equals(movie.Title, converted.Title));
            Assert.Equal(movie.RunningTime.Value, converted.RunningTime);
            Assert.Equal(DateTime.Now.Year - movie.YearOfRelease, converted.YearsPassedSinceOriginalRelease);
        }
    }
}
