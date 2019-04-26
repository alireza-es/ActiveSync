namespace ActiveSync.Core.Requests.Handlers.ItemOperations
{
    public class MoveItemOperation
    {
        public string ConversationId { get; set; }
        public string DstFldId { get; set; }
        public MoveItemOperationOption Options { get; set; }
    }

    public class MoveItemOperationOption
    {
        public bool MoveAlways { get; set; }
    }
}
