using System;

namespace MovieAPI.Models
{
    public class AddUpdateReview
    {
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

        public bool IsValidForSubmission()
        {
            return ReviewerId != 0 & MovieId != 0 & Score != 0;
        }

        public override string ToString()
        {
            return $"ReviewerId was {ReviewerId}, MovieId was {MovieId}, Score was {Score}";

        }
    }
}
