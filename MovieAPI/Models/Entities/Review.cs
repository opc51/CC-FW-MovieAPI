using MovieAPI.Models.DTOs.Outputs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.Entities
{
    /// <summary>
    /// A movie review containing the Reviewer id, the movie id and the score
    /// </summary>
    public class Review
    {
        /// <summary>
        /// The primary key of the movie review. Type <see cref="int"/>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The primary key of the reviewer. Type <see cref="int"/>
        /// </summary>
        [Required]
        public int ReviewerId { get; set; }

        /// <summary>
        /// The primary key of the movie. Type <see cref="int"/>
        /// </summary>
        public int MovieId { get; set; }

        /// <summary>
        /// The Movie of type <see cref="Movie"/> that is the parent of this review
        /// </summary>
        public Movie Movie { get; set; }

        /// <summary>
        /// The score for the Movie. Type <see cref="int"/>
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// A private constructor to prevent invalid objects being created
        /// </summary>
        private Review(int reviewerId, int movieId, int score) {
            ReviewerId = reviewerId;
            MovieId = movieId;
            Score = score;
        }

        /// <summary>
        /// Used to create movie Review records
        /// </summary>
        /// <param name="reviewerId">The primary key of the reviewer. Type <see cref="int"/></param>
        /// <param name="movieId">The rpimary key of the movie. Type <see cref="int"/></param>
        /// <param name="score">The score given to the movie. Minimum value 1, maximum value 5. Type <see cref="int"/></param>
        public static Review Create(int reviewerId, int movieId, int score)
        {
            if(!IsScoreValid(score))
            {
                var errorMessage = $"The score provided for the review was {score}. This is invalid. Score must be between 1 and 5.";
                throw new ArgumentOutOfRangeException(errorMessage);
            }

            if (!AreReferencesValid(reviewerId, movieId))
            {
                var errorMessage = $"The reviewer Id or the movieIds provided was zero. Please provie the real reviewer and movie Ids.";
                throw new ArgumentException(errorMessage);
            }
            return new Review(reviewerId, movieId, score);
        }

        #region APIs

        private static bool IsScoreValid(int score)
        {
            if (score < 1 || score > 5)
            {
                return false;
            }
            return true;
        }

        private static bool AreReferencesValid(int reviewerId, int movieId)
        {
            if (reviewerId == 0 || movieId == 0)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
