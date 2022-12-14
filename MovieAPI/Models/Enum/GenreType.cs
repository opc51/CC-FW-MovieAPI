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
        /// The recognised movie genre types
        /// </summary>
        /// <param name="name">The name of the Genre. Type <see cref="string"/></param>
        /// <param name="value">The <see cref="int"/> value of the movie genre. 
        /// Sub genres share the same integer value as the parent genre.</param>
        public GenreType(string name, int value) : base(name, value) { }

        #region UnknownType

        /// <summary>
        /// Use this option when the Movie does not match an existing type of <see cref="GenreType"/>
        /// </summary>
        public static readonly GenreType Unknown = new GenreType(nameof(Unknown), 0);

        #endregion

        #region SuperHero

        /// <summary>
        /// Examples, Marvel and DC Movies
        /// </summary>
        public static readonly GenreType SuperHero = new GenreType(nameof(SuperHero), 1);

        /// <summary>
        /// A sub group of <see cref="SuperHero"/>
        /// </summary>
        public static readonly GenreType Hero = new GenreType(nameof(Hero), 1);

        /// <summary>
        /// A sub group of <see cref="SuperHero"/>
        /// </summary>
        public static readonly GenreType Heros = new GenreType(nameof(Heros), 1);

        /// <summary>
        /// A sub group of <see cref="SuperHero"/>
        /// </summary>
        public static readonly GenreType Marvel = new GenreType(nameof(Marvel), 1);

        /// <summary>
        /// A sub group of <see cref="SuperHero"/>
        /// </summary>
        public static readonly GenreType DC = new GenreType(nameof(DC), 1);

        #endregion

        #region Comedy

        /// <summary>
        /// Examples include Airplane, Police Academy
        /// </summary>
        public static readonly GenreType Comedy = new GenreType(nameof(Comedy), 2);

        /// <summary>
        /// A sub group of <see cref="Comedy"/>
        /// </summary>
        public static readonly GenreType Funny = new GenreType(nameof(Funny), 2);

        /// <summary>
        /// A sub group of <see cref="Comedy"/>
        /// </summary>
        public static readonly GenreType Fun = new GenreType(nameof(Fun), 2);

        #endregion

        #region Romance

        /// <summary>
        /// Examples include "Romancing the Stone"
        /// </summary>
        public static readonly GenreType Romance = new GenreType(nameof(Romance), 3);

        /// <summary>
        /// A sub group of <see cref="Romance"/>
        /// </summary>
        public static readonly GenreType Weepy = new GenreType(nameof(Weepy), 3);

        /// <summary>
        /// A sub group of <see cref="Comedy"/>
        /// </summary>
        public static readonly GenreType Romantic = new GenreType(nameof(Romantic), 3);

        #endregion

        #region Action

        /// <summary>
        /// Examples include Terminator, Rambo
        /// </summary>
        public static readonly GenreType Action = new GenreType(nameof(Action), 4);

        /// <summary>
        /// A sub group of <see cref="Action"/>
        /// </summary>
        public static readonly GenreType Adventure = new GenreType(nameof(Adventure), 4);

        /// <summary>
        /// A sub group of <see cref="Action"/>
        /// </summary>
        public static readonly GenreType MartialArts = new GenreType(nameof(MartialArts), 4);

        #endregion

        #region SciFi

        /// <summary>
        /// Examples include Aliens, 2001
        /// </summary>
        public static readonly GenreType SciFi = new GenreType(nameof(SciFi), 5);

        /// <summary>
        /// A sub group of <see cref="SciFi"/>
        /// </summary>
        public static readonly GenreType ScienceFiction = new GenreType(nameof(ScienceFiction), 5);

        #endregion
    }
}
