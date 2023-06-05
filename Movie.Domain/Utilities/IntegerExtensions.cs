namespace Movie.Domain.Utilites
{
    /// <summary>
    /// Extension on type <see cref="int"/>
    /// </summary>
    public static class IntExtension
    {
        /// <summary>
        /// Checks that the given integer is positive and non-zero integers
        /// </summary>
        /// <param name="value">The <see cref="int"/> in context</param>
        /// <returns>True is postive and non-zero. False otherwise.</returns>
        public static bool IsPositiveAndNonZeroInteger(this int value)
        {
            if (value < 1)
            {
                return false;
            }
            return true;
        }
    }
}
