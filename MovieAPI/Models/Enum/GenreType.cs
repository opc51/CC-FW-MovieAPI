using Ardalis.SmartEnum;

namespace MovieAPI.Models.Enum
{
    /// <summary>
    /// Enum for the movie genre type. 
    /// 
    /// This implementation uses Ardalis SmartEnums this means that different names can be assigned to
    /// the same value .e.g. SuperHero, Hero, Heros are different names for the same underlying group
    /// </summary>
    public sealed class GenreType : SmartEnum<GenreType>
    {
        /// <summary>
        /// Used to generate a new Genre Type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public GenreType(string name, int value) : base(name, value) { }

        /// <summary>
        /// Use this option when the Movie does not match an existing Genre
        /// </summary>
        public static readonly GenreType Unknown = new GenreType(nameof(Unknown), 0);

        /// <summary>
        /// Examples, Marvel and DC Movies
        /// </summary>
        public static readonly GenreType SuperHero = new GenreType(nameof(SuperHero), 1);
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly GenreType Hero = new GenreType(nameof(Hero), 1);
        public static readonly GenreType Heros = new GenreType(nameof(Heros), 1);
        public static readonly GenreType Marvel = new GenreType(nameof(Marvel), 1);
        public static readonly GenreType DC = new GenreType(nameof(DC), 1);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


        /// <summary>
        /// Examples include Airplane, Police Academy
        /// </summary>
        public static readonly GenreType Comedy = new GenreType(nameof(Comedy), 2);
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly GenreType Funny = new GenreType(nameof(Funny), 2);
        public static readonly GenreType Fun = new GenreType(nameof(Fun), 2);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Examples include "Romancing the Stone"
        /// </summary>
        public static readonly GenreType Romance = new GenreType(nameof(Romance), 3);
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly GenreType Weepy = new GenreType(nameof(Weepy), 3);
        public static readonly GenreType Romantic = new GenreType(nameof(Romantic), 3);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Examples include Terminator, Rambo
        /// </summary>
        public static readonly GenreType Action = new GenreType(nameof(Action), 4);
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly GenreType Adventure = new GenreType(nameof(Adventure), 4);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


        /// <summary>
        /// Examples include Aliens, 2001
        /// </summary>
        public static readonly GenreType SciFi = new GenreType(nameof(SciFi), 5);
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly GenreType ScienceFiction = new GenreType(nameof(ScienceFiction), 5);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
