using System;
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

        private int reviewerId;
        /// <summary>
        /// The primary key of the reviewer. Type <see cref="int"/>
        /// </summary>
        [Required]
        public int ReviewerId
        {
            get
            {
                return reviewerId;
            }

            set
            {
                if (!AreIntegersPostiveAndNonZero(value))
                {
                    var errorMessage = "The movie Id must be a non zero and positive integer";
                    throw new ArgumentOutOfRangeException(errorMessage);
                }
                reviewerId = value;
            }

        }

        private int movieId;
        /// <summary>
        /// The primary key of the movie. Type <see cref="int"/>
        /// </summary>
        public int MovieId
        {
            get
            {
                return movieId;
            }

            set
            {
                if (!AreIntegersPostiveAndNonZero(value))
                {
                    var errorMessage = "The movie Id must be a non zero and positive integer";
                    throw new ArgumentOutOfRangeException(errorMessage);
                }
                movieId = value;
            }
        }

        /// <summary>
        /// The Movie of type <see cref="Movie"/> that is the parent of this review
        /// </summary>
        public Movie Movie { get; set; }

        private int score;
        /// <summary>
        /// The score for the Movie. Type <see cref="int"/>
        /// </summary>
        public int Score
        {
            get
            {
                return score;
            }

            set
            {
                if (!IsScoreValid(value))
                {
                    var errorMessage = $"The score provided for the review was {score}. This is invalid. Score must be between 1 and 5.";
                    throw new ArgumentOutOfRangeException(errorMessage);
                }
                score = value;
            }

        }

        /// <summary>
        /// A private constructor to prevent invalid objects being created
        /// </summary>
        private Review() { }

        /// <summary>
        /// Used to create movie Review records
        /// </summary>
        /// <param name="reviewerId">The primary key of the reviewer. Type <see cref="int"/></param>
        /// <param name="movieId">The primary key of the movie. Type <see cref="int"/></param>
        /// <param name="score">The score given to the movie. Minimum value 1, maximum value 5. Type <see cref="int"/></param>
        public static Review Create(int reviewerId, int movieId, int score)
        {
            return new Review()
            {
                ReviewerId = reviewerId,
                MovieId = movieId,
                Score = score
            };
        }

        #region APIs

        /// <summary>
        /// Checks if the score is an <see cref="int"/> between 1 and 5 inclusive
        /// </summary>
        /// <param name="score">The score assigned by the reviewer. Type Int <see cref=""/></param>
        /// <returns>True if score is valid. False if invalid</returns>
        public static bool IsScoreValid(int score)
        {
            if (score < 1 || score > 5)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Checks that the given integer value is positive and non-zero integers
        /// </summary>
        /// <param name="value">The <see cref="int"/> id of the Reviewer</param>
        /// <returns>True is postive and non-zero. False otherwise.</returns>
        public static bool AreIntegersPostiveAndNonZero(int value)
        {
            if (value < 1)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
