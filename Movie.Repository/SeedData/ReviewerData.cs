using Movie.Repository.Entities;

namespace Movie.Repository.SeedData
{
    public static class ReviewerData
    {
        public static List<Reviewer> Fetch()
        {
            return new List<Reviewer>() {
                    Reviewer.Create("JohnTheBrit", "john@john.com", "gb", "01234875456", 1),
                    Reviewer.Create("JaneAmerican", "john@john.com", "US", "3333334444", 2),
                    Reviewer.Create("JoseyFrance", "john@john.com", "Fr", "123456789", 3),
            };
        }
    }
}
