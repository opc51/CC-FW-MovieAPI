//using System;

//namespace MovieAPI.Models.Entities.Common
//{
//    /// <summary>
//    /// Used to handle the creation and validation of email addresses
//    /// </summary>
//    public class EmailAddress
//    {
//        /// <summary>
//        /// The email address as type <see cref="string"/>
//        /// </summary>
//        public string Value { get; private set; }

//        private EmailAddress(string email)
//        {
//            Value = email;
//        }

//        /// <summary>
//        /// Used to create an instance of type <see cref="EmailAddress"/>
//        /// </summary>
//        /// <param name="email">A <see cref="string"/> that is the email address</param>
//        /// <returns>A new instance of type <see cref="EmailAddress"/></returns>
//        public static EmailAddress Create(string email)
//        {
//            if (!IsEmailAddressValid(email))
//            {
//                var error = $"the text {email} is not a valid email address";
//                throw new ArgumentException(error);
//            }
//            return new EmailAddress(email);
//        }

//        /// <summary>
//        /// Used to determine if the string provided is in the right format for an email
//        /// </summary>
//        /// <param name="email">A <see cref="string"/> that represents the email address</param>
//        /// <returns></returns>
//        public static bool IsEmailAddressValid(string email)
//        {
//            if (string.IsNullOrEmpty(email))
//            {
//                return false;
//            }
//            if (!MatchesEmailRegex(email))
//            {
//                return false;
//            }
//            return true;
//        }

//        private static bool MatchesEmailRegex(string email)
//        {
//            //new Regex("^\\S+@\\S+\\.\\S+$")

//            return true;
//            //throw new NotImplementedException();
//        }

//        #region TypeConversions

//        /// <summary>
//        /// Allows the automatic conversion of the type <see cref="EmailAddress"/> into type <see cref="string"/>
//        /// </summary>
//        /// <param name="e">Type of <see cref="EmailAddress"/></param>
//        public static implicit operator string(EmailAddress e) => e.Value;

//        /// <summary>
//        /// Allows the automatic conversion of type <see cref="string"/> into type <see cref="EmailAddress"/>
//        /// </summary>
//        /// <param name="e">Type of <see cref="string"/></param>
//        public static implicit operator EmailAddress(string e) => Create(e);

//        #endregion
//    }
//}
