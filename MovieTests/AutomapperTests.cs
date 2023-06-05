using AutoFixture;
using AutoMapper;
using Movie.API.AutoMapper;
using Movie.Domain;
using Movie.Domain.Enum;
using NUnit.Framework;
using Domain = Movie.Domain;
using System;

namespace MovieTests
{
    public class AutomapperTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _config;
        private readonly Fixture _fixture = new();

        public AutomapperTests()
        {
            _config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MovieProfiles>();
            });

            _mapper = _config.CreateMapper();
        }

        [Test]
        public void Config_Is_Valid()
        {
            _config.AssertConfigurationIsValid();
        }

        [Test]
        public void MovieEnity_convertsSuccesfully_toMovieDto()
        {
            //Arrange
            string movieName = _fixture.Create<string>();
            var movie = Domain.Movie.Create(movieName, ReleaseYear.Create(2000),
                                              RunningTime.Create(180), GenreType.Comedy);

            //Act
            Movie.Repository.Services.DTOs.Output.Movie converted = _mapper.Map<Domain.Movie, Movie.Repository.Services.DTOs.Output.Movie>(movie);

            //Assert
            Assert.That(movie.Title, Is.EqualTo(converted.Title));
            Assert.That(movie.RunningTime.Value, Is.EqualTo(converted.RunningTime));
            Assert.That(DateTime.Now.Year - movie.YearOfRelease, 
                            Is.EqualTo(converted.YearsPassedSinceOriginalRelease));
        }
    }
}
