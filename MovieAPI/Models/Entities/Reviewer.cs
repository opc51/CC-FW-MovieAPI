﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Mail;

namespace MovieAPI.Models.Entities
{
    /// <summary>
    /// Details of the person who reviewed the Movie
    /// </summary>
    [DebuggerDisplay("Name : {Name}, Email ; {Email}")]
    public class Reviewer
    {
        /// <summary>
        /// The primary key of the Id. Type of <see cref="int"/>
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The name of the reviewer. Type of <see cref="string"/>
        /// </summary>
        [Required]
        public string Name { get; set; }

        private string email;
        /// <summary>
        /// The email of the Reviewer. Type of <see cref="string"/>
        /// </summary>
        public string Email {
            get { 
                return email;
            }

            set { 
                // throws InvalidFormatException if email address is not valid
                var mailAddress = new MailAddress(value);
                email = mailAddress.Address;
            }
        }

        /// <summary>
        /// Private constructor to prevent public miss use of the object
        /// 
        /// Use the provide static Create method to instantiate new instances of <see cref="Reviewer"/>
        /// </summary>
        private Reviewer(string name, string email) {
            Name = name;
            Email = email;
        }

        /// <summary>
        /// Creates an instance of <see cref="Reviewer"/>
        /// </summary>
        /// <param name="name">The Reviewers Name <see cref="string"/></param>
        /// <param name="email">The Reviewers Email <see cref="string"/></param>
        /// <returns>A new instance of <see cref="Reviewer"/></returns>
        public static Reviewer Create(string name, string email)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Reviewer Name and Email cannot be empty when creating a reviewer");
            }

            return new Reviewer(name, email);
        }
    }
}
