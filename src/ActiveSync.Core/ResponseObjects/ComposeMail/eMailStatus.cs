namespace ActiveSync.Core.ResponseObjects.ComposeMail
{
    public enum eMailStatus:byte
    {
        /// <summary>
        /// Server successfully completed command.
        /// => Global
        /// </summary>
        Success = 1,
        ProtocolError = 2,
        ServerUnavailable = 4,
        InvalidArguments = 5,
        ConflictingArguments = 6,
        DeniedByPolicy = 7,
        
        
        ServerError = 110,
        MessageReplyFailed = 121,
        AccessDenied = 130,

    }
}
