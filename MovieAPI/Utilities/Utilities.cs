﻿using System;

namespace Movie.API
{
    /// <summary>
    /// Static Utility Classes. Useful functionality that could be needed in multiple places
    /// </summary>
    public static class Utilities
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
