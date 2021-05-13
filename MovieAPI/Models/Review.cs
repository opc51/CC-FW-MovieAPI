using System;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models
{
    public class Review
    {
        public Review()
        {

        }
        public Review(int reviewerId, int movieId, int score)
        {
            ReviewerId = reviewerId;
            MovieId = movieId;
            Score = score;
        }

        public int Id { get; set; }
        public int ReviewerId { get; set; }
        public int MovieId { get; set; }

        private int score;
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
