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
            // check date not befor first movie made

            // check not in the future

            throw new System.NotImplementedException();
        }
    }
}
