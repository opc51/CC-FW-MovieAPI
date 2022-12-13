using MovieAPI.Models.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Mail;

namespace MovieAPI.Models.Entities
{
    /// <summary>
    /// A reviwer that contains an id and a name
    /// </summary>
    [DebuggerDisplay("Name : {Name}, Email : {Email}")]
    public class Reviewer
    {
        /// <summary>
        /// Empty constructor needed for unit tests. However should be removed.
        /// </summary>
        private Reviewer(string name, string email) {
            Name = name;
            Email = new MailAddress(email);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static Reviewer Create(string name, string email)
        {
            // safety checks

            return new Reviewer(name, email);
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

        /// <summary>
        /// The reviewers email address
        /// </summary>
        public MailAddress Email { get; set; }
    }
}
