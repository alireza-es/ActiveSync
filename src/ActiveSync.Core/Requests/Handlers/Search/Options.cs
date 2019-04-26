using ActiveSync.Core.Requests.Handlers.Sync;

namespace ActiveSync.Core.Requests.Handlers.Search
{
    public class Options
    {
        public eMIMESendSupport MIMESupport { get; set; }
        public BodyPreference BodyPreference { get; set; }
        public BodyPartPreference BodyPartPreference { get; set; }
        public string Range { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool DeepTraversal { get; set; }
        public bool RebuildResults { get; set; }
        public Picture Picture { get; set; }
    }
    public class BodyPartPreference
    {
        public eBodyContentType Type { get; set; }
        public int? TruncationSize { get; set; }
        public bool AllOrNone { get; set; }
        /// <summary>
        /// specifies the maximum length of the Unicode plain text message or message part preview to be returned to the client.
        /// </summary>
        public int? Preview { get; set; }
    }

    public class Picture
    {
        public int MaxSize { get; set; }
        public int MaxPictures { get; set; }
    }
}
