using System;

namespace MovieAPI.Models.Entities.Common
{
    /// <summary>
    /// The year a movie was released
    /// </summary>
    public class ReleaseYear
    {
        public static int Value { get; private set; }
        private ReleaseYear()
        {

        }

        /// <summary>
        /// Calculates how many years ago it was since the movie was released
        /// </summary>
        /// <returns>The number of years since the movies release <<see cref="int"></returns>
        public static int NumberOfYearsAgoReleased()
        {
            return DateTime.Now.Year - Value;
        }

        private static bool IsValidReleaseYear(int value)
        {
            // Cannot be before the date movies were invented
            if (value < 1895)
            {
                return false;
            } 
            if (value > DateTime.Now.Year)
            {
                return false;
            }
            return true;
        }

        public static ReleaseYear Create(int value)
        {
            if (!IsValidReleaseYear(value))
            {
                var errorMessage = $"";
                throw new NotImplementedException(errorMessage);
            }
            return Create(value);
        }


    }
}
