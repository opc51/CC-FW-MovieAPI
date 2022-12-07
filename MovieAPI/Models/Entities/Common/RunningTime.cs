namespace MovieAPI.Models.Entities.Common
{
    public class RunningTime
    {
        public int Value { get; private set; }

        /// <summary>
        ///  Private constructor to prevent invalid object creation
        /// </summary>
        private RunningTime() { }

        private bool IsRunningTimeReasonable(int value)
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

        public static int ConvertHoursToMinutes()
        {
            throw new System.NotImplementedException();
        }

        public static int ConvertIntegerHoursToMinutes()
        {
            throw new System.NotImplementedException();
        }

        public static RunningTime Create(int value)
        {
            throw new System.NotImplementedException();
        }
    }
}
