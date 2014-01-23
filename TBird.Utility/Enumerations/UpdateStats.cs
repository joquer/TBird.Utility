namespace TBird.Utility.Enumerations
{
    /// <summary>
    /// UpdateStats class returns what changes were made to the database during
    /// the update process.
    /// </summary>
    public class UpdateStats
    {
        public int AddCount { get; set; }

        public int UpdateCount { get; set; }
    }
}
