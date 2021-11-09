using System;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models
{
    /// <summary>
    /// A movie review containing the Reviewer id, the movie id and the score
    /// </summary>
    public class Review
    {
        /// <summary>
        /// A public sconstructor, currently needed for unit tsting. This should be done in a different way
        /// </summary>
        public Review()
        {

        }

        /// <summary>
        /// a public constructor for a Movie Review
        /// </summary>
        /// <param name="reviewerId">The primary key of the reviewer</param>
        /// <param name="movieId">The rpimary key of the movie</param>
        /// <param name="score">The score given to the movie MInimum 1 , maximum 5)</param>
        public Review(int reviewerId, int movieId, int score)
        {
            ReviewerId = reviewerId;
            MovieId = movieId;
            Score = score;
        }

        /// <summary>
        /// The primary key of the movie review
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// The primary key of the reviewer
        /// </summary>
        [Required]
        public int ReviewerId { get; set; }


        /// <summary>
        /// The primary key of the movie
        /// </summary>
        [Required]
        public int MovieId { get; set; }


        private int score;
        /// <summary>
        /// The score given to the movie.
        /// 
        /// Has a minimum value of 1 and a maximum value of 5
        /// </summary>
        [Required]
        public int Score
        {
            get { return score; }
            set
            {
                if (value < 1 | value > 5)
                    throw new ArgumentOutOfRangeException("Value must be between 1 and 5", nameof(Score));
                else
                    score = value;
            }
        }
    }
}
