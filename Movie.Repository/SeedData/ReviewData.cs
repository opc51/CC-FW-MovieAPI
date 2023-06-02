using Movie.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Repository.SeedData
{
    public static class ReviewData
    {
        public static List<Review> Fetch()
        {
            return new List<Review>()
            {
                Review.Create(1, 1, 5), Review.Create(1, 2, 4), Review.Create(1, 3, 5),
                Review.Create(1, 4, 2), Review.Create(1, 5, 3), Review.Create(1, 6, 5),
                Review.Create(2, 1, 2), Review.Create(2, 2, 1), Review.Create(2, 3, 3),
                Review.Create(2, 4, 4), Review.Create(2, 5, 3), Review.Create(2, 6, 5),
                Review.Create(3, 1, 1), Review.Create(3, 2, 1), Review.Create(3, 3, 5),
                Review.Create(3, 4, 5), Review.Create(3, 5, 2), Review.Create(3, 6, 1)
            };
        }
    }
}
