namespace MovieAPI.Models
{
    public class Reviewer
    {
        public Reviewer()
        {

        }
        public Reviewer(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
