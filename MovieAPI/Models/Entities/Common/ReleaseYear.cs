using System;

namespace MovieAPI.Models.Entities.Common
{
    /// <summary>
    /// The year a movie was released.
    /// </summary>
    public class ReleaseYear : IEquatable<ReleaseYear>
    {
        /// <summary>
        /// The year that the movie was released
        /// </summary>
        public int Value { get; private set; }

        private ReleaseYear(int value){
            // private constructor to ensure only valid objects are created
            Value = value;
        }

        /// <summary>
        /// Method to create a new instance of type <see cref="ReleaseYear"/>
        /// </summary>
        /// <param name="year"></param>
        /// <returns>The year the movie was released. Type of <see cref="int"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ReleaseYear Create(int year)
        {
            if (!IsValidReleaseYear(year))
            {
                var errorMessage = $"The year {year} cannot be used. Valid values are between 1895 and this year";
                throw new ArgumentException(errorMessage);
            }
            return new ReleaseYear(year);
        }

        #region APIs

        /// <summary>
        /// Calculates how many years ago it was since the movie was released
        /// </summary>
        /// <returns>The number of years since the movies release. Type of <see cref="int"/></returns>
        public int NumberOfYearsAgoReleased()
        {
            return DateTime.Now.Year - Value;
        }

        /// <summary>
        /// Determines if the release year is between 1895 (the year of the first cinema release)
        /// and this year.
        /// </summary>
        /// <param name="year">The year the movie was released <see cref="int"/></param>
        /// <returns>True if valid. False otherwise.</returns>
        private static bool IsValidReleaseYear(int year)
        {
            // Cannot be before the date movies were invented
            if (year < 1895)
            {
                return false;
            } 
            // cannot be in the future
            if (year > DateTime.Now.Year)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region

        /// <summary>
        /// Allows the automatic conversion of the type <see cref="ReleaseYear"/> into type <see cref="int"/>
        /// </summary>
        /// <param name="y">Type of <see cref="ReleaseYear"/></param>
        public static implicit operator int(ReleaseYear y) => y.Value;

        /// <summary>
        /// Allows the automatic conversion of type <see cref="int"/> into type <see cref="ReleaseYear"/>
        /// </summary>
        /// <param name="y">Type of <see cref="int"/></param>
        public static implicit operator ReleaseYear(int y) => Create(y);

        #endregion

        #region IEquatable Members

        /// <summary>
        /// Required object equality operator for type <see cref="Object"/>
        /// </summary>
        /// <param name="obj">Type of <see cref="object"/></param>
        /// <returns>True if equal false otherwise</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ReleaseYear);
        }

        /// <summary>
        /// Required object equality operator for type <see cref="ReleaseYear"/>
        /// </summary>
        /// <param name="other">Type of <see cref="ReleaseYear"/></param>
        /// <returns>True if equal false otherwise</returns>
        public bool Equals(ReleaseYear other)
        {
            return other != null &&
                Value.Equals(other.Value);
        }

        /// <summary>
        /// The required GetHashCode overide equality operator on type <see cref="ReleaseYear"/>
        /// </summary>
        /// <returns>The Hashcode that represents the single value</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        /// <summary>
        /// Required Equality operator for type <see cref="ReleaseYear"/>
        /// </summary>
        /// <param name="x">First <see cref="ReleaseYear"/></param>
        /// <param name="y">Second <see cref="ReleaseYear"/></param>
        /// <returns>True is they are equal. False otherwise</returns>
        public static bool operator ==(ReleaseYear x, ReleaseYear y)
        {
            return Equals(x,y);
        }

        /// <summary>
        /// Required Inequality operator for type <see cref="ReleaseYear"/>
        /// </summary>
        /// <param name="x">First <see cref="ReleaseYear"/></param>
        /// <param name="y">Second <see cref="ReleaseYear"/></param>
        /// <returns>True is they are equal. False otherwise</returns>
        public static bool operator !=(ReleaseYear x, ReleaseYear y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
