using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAPI
{
    public static class Utilities
    {
        /// <summary>
        /// This function will round a float to the nearest 0.5 . In the case of a value being equidistant
        /// from both values it shall be rounded up
        /// </summary>
        /// <returns></returns>
        public static double RoundToTheNearestHalf(double input)
        {
            var digits = (int)input;
            var decimals = input - digits;

            if (decimals < 0.25)
            {
                return digits;
            } 
            else if (decimals >= 0.25 && decimals <0.75)
            {
                return digits + 0.5;
            } else
            {
                return digits + 1;
            }
        }
    }
}
