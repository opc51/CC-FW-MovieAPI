using System;

namespace MovieAPI.Models.Entities.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class RunningTime : IEquatable<RunningTime>
    {
        /// <summary>
        /// The running time of the movie in minutes. Type <see cref="int"/>
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        ///  Private constructor to prevent invalid object creation
        /// </summary>
        /// <param name="length">Type of <see cref="int"/></param>
        private RunningTime(int length) {
            Value = length;
        }

        /// <summary>
        /// Used to create an instance of type <see cref="RunningTime"/>
        /// </summary>
        /// <param name="movieLength">Runtime in minutes. Type <see cref="int"/></param>
        /// <returns>An instance of type <see cref="RunningTime"/></returns>
        /// <exception cref="System.ArgumentException">Thrown if movie length is less than 1 minute or greater than 24 hours</exception>
        public static RunningTime Create(int movieLength)
        {
            if (!IsRunningTimeReasonable(movieLength))
            {
                var errorMessage = $"The movie cannot be more than 24 hours or less than 1 minute";
                throw new ArgumentException(errorMessage);
            }
            return new RunningTime(movieLength);
        }

        /// <summary>
        /// Used to create an instance of type <see cref="RunningTime"/>
        /// </summary>
        /// <param name="movieLength">Runtime in minutes. Type <see cref="float"/></param>
        /// <returns>An instance of type <see cref="RunningTime"/></returns>
        /// <exception cref="System.ArgumentException">Thrown if movie length is less than 1 minute or greater than 24 hours</exception>
        public static RunningTime Create(float movieLength)
        {
            int intLength = ConvertHoursToMinutes(movieLength);
            return Create(intLength);
        }

        private static bool IsRunningTimeReasonable(int value)
        {
            // Running time cannot be less than 1 minute
            if (value < 1)
            {
                return false;
            }
            //running time cannot be more than 24 hours
            if (value > 1440)
            {
                return false;
            }
            return true;
        }

        private static int ConvertHoursToMinutes(float time)
        {
            return (int) (time * 60);
        }

        #region IEquatable Members

        /// <summary>
        /// Required object equality operator for type <see cref="object"/>
        /// </summary>
        /// <param name="obj">Type of <see cref="object"/></param>
        /// <returns>True if equal false otherwise</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as RunningTime);
        }

        /// <summary>
        /// Required object equality operator for type <see cref="RunningTime"/>
        /// </summary>
        /// <param name="other">Type of <see cref="RunningTime"/></param>
        /// <returns>True if equal false otherwise</returns>
        public bool Equals(RunningTime other)
        {
            return other != null &&
                Value.Equals(other.Value);
        }

        /// <summary>
        /// The required GetHashCode overide equality operator on type <see cref="RunningTime"/>
        /// </summary>
        /// <returns>The Hashcode that represents the single value</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        /// <summary>
        /// Allows the automatic conversion of the type <see cref="RunningTime"/> into type <see cref="int"/>
        /// </summary>
        /// <param name="y">Type of <see cref="RunningTime"/></param>
        public static implicit operator int(RunningTime y) => y.Value;

        /// <summary>
        /// Allows the automatic conversion of type <see cref="int"/> into type <see cref="RunningTime"/>
        /// </summary>
        /// <param name="y">Type of <see cref="int"/></param>
        public static explicit operator RunningTime(int y) => Create(y);

        /// <summary>
        /// Required Equality operator for type <see cref="RunningTime"/>
        /// </summary>
        /// <param name="x">First <see cref="RunningTime"/></param>
        /// <param name="y">Second <see cref="RunningTime"/></param>
        /// <returns>True is they are equal. False otherwise</returns>
        public static bool operator ==(RunningTime x, RunningTime y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// Required Inequality operator for type <see cref="RunningTime"/>
        /// </summary>
        /// <param name="x">First <see cref="RunningTime"/></param>
        /// <param name="y">Second <see cref="RunningTime"/></param>
        /// <returns>True is they are equal. False otherwise</returns>
        public static bool operator !=(RunningTime x, RunningTime y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
