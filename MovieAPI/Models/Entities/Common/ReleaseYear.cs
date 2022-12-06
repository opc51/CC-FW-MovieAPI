using System;

namespace MovieAPI.Models.Entities.Common
{
    public class ReleaseYear
    {
        public int Value { get; private set; }

        public ReleaseYear(int value)
        {
            if (!IsValidReleaseYear(value))
            {
                // thow my own exception
                var errorMessage = $"";
                throw new System.Exception(errorMessage);
            }

            Value = value;
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
    }
}
