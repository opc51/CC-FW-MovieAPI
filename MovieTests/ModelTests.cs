using AutoFixture;
using Movie.API.Models;
using Movie.Respository.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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
        [TestCaseSource(nameof(invalidSearchCriteria))]
        public void MovieSearchCriteriaShould_beInvalidWithNullData(string title, string genre, int year)
        {
            MovieSearchCriteria sut = new() { Title = title, Genre = genre, Year = year };
            Assert.False(sut.IsValid());
        }


        [Theory]
        [TestCaseSource(nameof(validSearchCriteria))]
        public void MovieSearchCriteriaShould_beValidWithGoodData(string title, string genre, int year)
        {
            MovieSearchCriteria sut = new() { Title = title, Genre = genre, Year = year };
            Assert.True(sut.IsValid());
        }

        [Theory]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void AddUpdateReviewShould_AllowsValuesBetween1and5(int score)
        {
            AddUpdateReview sut = new()
            {
                MovieId = fixture.Create<int>(),
                ReviewerId = fixture.Create<int>(),
                Score = score
            };
            Assert.That(score, Is.EqualTo(sut.Score));
        }


        [Theory]
        [TestCase(0)]
        [TestCase(6)]
        [TestCase(7)]
        public void AddUpdateReviewShould_NotAllowsValuesOutside1and5(int score)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new AddUpdateReview() { Score = score });
        }

        [Theory]
        [TestCaseSource(nameof(invalidReviewSubmissions))]
        public void ReviewModelShould_ReturnFalseForInvalidSubmissions(int reviewerId, int movieId, int score)
        {
            AddUpdateReview sut = new() { ReviewerId = reviewerId, MovieId = movieId, Score = score };
            Assert.False(sut.IsValidForSubmission());
        }


        [Theory]
        [TestCaseSource(nameof(validReviewSubmissions))]
        public void ReviewModelShould_ReturnTrueForValidSubmission(int reviewerId, int movieId, int score)
        {
            AddUpdateReview sut = new() { ReviewerId = reviewerId, MovieId = movieId, Score = score };
            Assert.True(sut.IsValidForSubmission());
        }
    }
}
