namespace MovieAPI.Models
{
    public class MovieSearchCriteria
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Title) || Year != 0 || !string.IsNullOrWhiteSpace(Genre);
        }

        public override string ToString()
        {
            return $"Title : {Title}, Year : {Year}, Genre {Genre}";
        }
    }
}
