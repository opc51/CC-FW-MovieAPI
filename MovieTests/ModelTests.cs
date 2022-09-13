using AutoFixture;
using MovieAPI.Models;
using MovieAPI.Models.Entities;
using MovieAPI.Models.Enum;
using System;
using System.Collections.Generic;
using Xunit;

namespace MovieTests
{
    public class ModelTests
    {
        public static IEnumerable<object[]> invalidSearchCriteria = new List<object[]>()
                                                {
                                                    new object[] { null,  null, 0 },
                                                    new object[] { "", null,  0 },
                                                    new object[] { null, "",  0 },
                                                    new object[] { "", "",  0 },
                                                };

        public static IEnumerable<object[]> validSearchCriteria = new List<object[]>()
                                                {
                                                    new object[] { "Move 1", null, 0 },
                                                    new object[] { "Movie 1 ", "",  0 },
                                                    new object[] { "Movie 1 ", "genre",  0 },
                                                    new object[] { "Movie 1 ", "",  1 },
                                                    new object[] { null, "Genre 1",  0 },
                                                    new object[] { "", "Genre 1",  0 },
                                                    new object[] { "Movie 1", "Genre 1",  1 },
                                                };

        public static IEnumerable<object[]> invalidReviewSubmissions = new List<object[]>()
                                                {
                                                    new object[]{ 0, 0, 1},
                                                    new object[]{ 0, 1, 1},
                                                    new object[]{ 1, 0, 1},
                                                };


        public static IEnumerable<object[]> validReviewSubmissions = new List<object[]>()
                                                {
                                                    new object[]{ 1, 1, 1}
                                                };

        private Fixture fixture = new();

        [Theory]
        [MemberData(nameof(invalidSearchCriteria))]
        public void MovieSearchCriteriaShould_beInvalidWithNullData(string title, string genre, int year)
        {
            MovieSearchCriteria sut = new() { Title = title, Genre = genre, Year = year };
            Assert.False(sut.IsValid());
        }


        [Theory]
        [MemberData(nameof(validSearchCriteria))]
        public void MovieSearchCriteriaShould_beValidWithGoodData(string title, string genre, int year)
        {
            MovieSearchCriteria sut = new() { Title = title, Genre = genre, Year = year };
            Assert.True(sut.IsValid());
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void ReviewerShould_AllowsValuesBetween1and5(int score)
        {
            Review sut = new(1, 1, score);
            Assert.Equal(score, sut.Score);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(7)]
        public void ReviewerShould_NotAllowsValuesOutside1and5(int score)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Review()
            {
                Id = fixture.Create<int>(),
                MovieId = fixture.Create<int>(),
                ReviewerId = fixture.Create<int>(),
                Score = score
            });
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void AddUpdateReviewShould_AllowsValuesBetween1and5(int score)
        {
            AddUpdateReview sut = new() 
            { 
                MovieId = fixture.Create<int>(),
                ReviewerId = fixture.Create<int>(),
                Score = score 
            };
            Assert.Equal(score, sut.Score);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(7)]
        public void AddUpdateReviewShould_NotAllowsValuesOutside1and5(int score)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new AddUpdateReview() { Score = score });
        }

        [Theory]
        [MemberData(nameof(invalidReviewSubmissions))]
        public void ReviewModelShould_ReturnFalseForInvalidSubmissions(int reviewerId, int movieId, int score)
        {
            AddUpdateReview sut = new() { ReviewerId = reviewerId, MovieId = movieId, Score = score };
            Assert.False(sut.IsValidForSubmission());
        }


        [Theory]
        [MemberData(nameof(validReviewSubmissions))]
        public void ReviewModelShould_ReturnTrueForValidSubmission(int reviewerId, int movieId, int score)
        {
            AddUpdateReview sut = new() { ReviewerId = reviewerId, MovieId = movieId, Score = score };
            Assert.True(sut.IsValidForSubmission());
        }


        [Fact]
        public void GenreTypes_Worksas_expected()
        {
            var myenum = GenreType.FromName("SuperHero");
            Assert.Equal(myenum, GenreType.SuperHero);
        }


    }
}
