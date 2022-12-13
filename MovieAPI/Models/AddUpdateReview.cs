using System;

namespace MovieAPI.Models
{
    /// <summary>
    /// Used to add or update movie reviews. 
    /// 
    /// Contains reviewer ids, movie id and a score
    /// </summary>
    public class AddUpdateReview
    {
        /// <summary>
        /// The primary key of the Reviewer
        /// </summary>
        public int ReviewerId { get; set; }

        /// <summary>
        /// The primary key of the movie
        /// </summary>
        public int MovieId { get; set; }

        private int score;
        /// <summary>
        /// An integer score with a minimum value of 1 and a maximum value of 5
        /// </summary>
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

        /// <summary>
        /// A method to check that the objct has valid data
        /// </summary>
        /// <returns></returns>
        public bool IsValidForSubmission()
        {
            return ReviewerId != 0 & MovieId != 0 & Score != 0;
        }

        /// <summary>
        /// overriden versions that outputs the movie Id, reviewer id and the score
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"ReviewerId was {ReviewerId}, MovieId was {MovieId}, Score was {Score}";
        }
    }
}
