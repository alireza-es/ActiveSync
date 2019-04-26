namespace ActiveSync.Core.Requests.Handlers.Sync
{
    public enum eMIMETruncation : byte
    {
        /// <summary>
        /// Truncate all body text.
        /// </summary>
        TruncateAll = 0,
        /// <summary>
        /// Truncate text over 4,096 characters.
        /// </summary>
        Truncate4K = 1,
        /// <summary>
        /// Truncate text over 5,120 characters.
        /// </summary>
        Truncake5K = 2,
        /// <summary>
        /// Truncate text over 7,168 characters.
        /// </summary>
        Truncake7K = 3,
        /// <summary>
        /// Truncate text over 10,240 characters.
        /// </summary>
        Truncake10K = 4,
        /// <summary>
        /// Truncate text over 20,480 characters.
        /// </summary>
        Truncake20K = 5,
        /// <summary>
        /// Truncate text over 51,200 characters.
        /// </summary>
        Truncake50K = 6,
        /// <summary>
        /// Truncate text over 102,400 characters.
        /// </summary>
        Truncake100K = 7,
        /// <summary>
        /// Do not truncate; send complete MIME data.
        /// </summary>
        NoTruncate = 8
    }
}