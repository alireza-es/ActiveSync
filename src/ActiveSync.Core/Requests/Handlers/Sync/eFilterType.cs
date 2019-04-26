namespace ActiveSync.Core.Requests.Handlers.Sync
{
    public enum eFilterType : byte
    {
        NoFilter = 0,
        _1Day = 1,
        _3Days = 2,
        _1Week = 3,
        _2Weeks = 4,
        _1Month = 5,
        _3Months = 6,
        _InCompleteTasks = 7
    }
}