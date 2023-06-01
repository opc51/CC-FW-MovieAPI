using AutoFixture;
using AutoMapper;
using Movie.API.AutoMapper;
using Movie.Repository.Entities.Common;
using Movie.Repository.Entities.Enum;
using System;
using Xunit;
using Entities = Movie.Repository.Entities;

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
            Movie.Repository.Services.DTOs.Output.Movie converted = _mapper.Map<Entities.Movie, Movie.Repository.Services.DTOs.Output.Movie>(movie);

            //Assert
            Assert.True(string.Equals(movie.Title, converted.Title));
            Assert.Equal(movie.RunningTime.Value, converted.RunningTime);
            Assert.Equal(DateTime.Now.Year - movie.YearOfRelease, converted.YearsPassedSinceOriginalRelease);
        }
    }
}
