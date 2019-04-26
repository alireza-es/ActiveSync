namespace ActiveSync.Core.Requests.Handlers.Sync
{
    public class Options
    {
        public eFilterType FilterType { get; set; }
        public eConflictPriority Conflict { get; set; }
        public eMIMETruncation MIMETruncation { get; set; }
        public eMIMESendSupport MIMESupport{ get; set; }
        public BodyPreference BodyPreference { get; set; }
    }

    public class BodyPreference
    {
        public eBodyContentType Type { get; set; }
        public int? TruncationSize { get; set; }
        public bool AllOrNone { get; set; }
        /// <summary>
        /// specifies the maximum length of the Unicode plain text message or message part preview to be returned to the client.
        /// </summary>
        public int? Preview { get; set; }
    }
}