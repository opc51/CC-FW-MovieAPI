using Ardalis.SmartEnum;

namespace MovieAPI.Models.Enum
{
    /// <summary>
    /// Enum for the movie genre type
    /// </summary>
    public sealed class GenreType : SmartEnum<GenreType>
    {
        /// <summary>
        /// Used to generate a new Genre Type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public GenreType(string name, int value) : base(name, value)
        {

        }

        /// <summary>
        /// Use this option when the Movie does not match an existing Genre
        /// </summary>
        public static readonly GenreType Unknown = new GenreType(nameof(Unknown), 0);

        /// <summary>
        /// Examples, Marvel and DC Movies
        /// </summary>
        public static readonly GenreType SuperHero = new GenreType(nameof(SuperHero), 1);
        public static readonly GenreType Hero = new GenreType(nameof(Hero), 1);

        /// <summary>
        /// Examples include Airplane, Police Academy
        /// </summary>
        public static readonly GenreType Comedy = new GenreType(nameof(Comedy), 2);

        /// <summary>
        /// Examples include "Romancing the Stone"
        /// </summary>
        public static readonly GenreType Romance = new GenreType(nameof(Romance), 3);

        /// <summary>
        /// Examples include Terminator, Rambo
        /// </summary>
        public static readonly GenreType Action = new GenreType(nameof(Action), 4);

        /// <summary>
        /// Examples include Aliens, 2001
        /// </summary>
        public static readonly GenreType SciFi = new GenreType(nameof(SciFi), 5);
    }
}
