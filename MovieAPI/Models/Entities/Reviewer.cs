using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MovieAPI.Models.Entities
{
    /// <summary>
    /// A reviwer that contains an id and a name
    /// </summary>
    [DebuggerDisplay("Name : {Name}")]
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
        /// <param name="name">The name of the reviewer</param>
        public Reviewer(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The primary key of the Id
        /// </summary>
        [Required]
        public int Id { get; set; }


        /// <summary>
        /// The name of the reviewer
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
