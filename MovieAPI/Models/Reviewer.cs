namespace MovieAPI.Models
{
    /// <summary>
    /// A reviwer that contains an id and a name
    /// </summary>
    public class Reviewer
    {
        /// <summary>
        /// Empty constructor needed for unit tests. However should be removed.
        /// </summary>
        public Reviewer()
        {

        }
        /// <summary>
        /// Constructor with name only
        /// </summary>
        /// <param name="name"></param>
        public Reviewer(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The primary key of the Id
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// The name of the reviewer
        /// </summary>
        public string Name { get; set; }
    }
}
