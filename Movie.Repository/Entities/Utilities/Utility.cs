namespace Movie.Repository.Entities.Utilites
{
    /// <summary>
    /// Static Utility Classes. Useful functionality that could be needed in multiple places
    /// 
    /// To do - move this
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// This function will round a float to the nearest 0.5. In the case of a value being equidistant
        /// from both values it shall be rounded up
        /// </summary>
        /// <returns><see cref="double"/></returns>
        public static double RoundToTheNearestHalf(double input)
        {
            return Math.Round(input * 2, MidpointRounding.AwayFromZero) / 2;
        }
    }
}
