namespace MovieAPI.Models
{
    public class Movie
    {
        public Movie()
        {

        }

        public Movie(string title, int yearOfRelease, int runnigTime, string genre)
        {
            Title = title;
            YearOfRelease = yearOfRelease;
            RunningTime = runnigTime;
            Genre = genre;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
        public int RunningTime { get; set; }
        public string Genre { get; set; }
    }
}
