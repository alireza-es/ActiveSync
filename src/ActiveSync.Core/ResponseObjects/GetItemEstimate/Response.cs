namespace ActiveSync.Core.ResponseObjects.GetItemEstimate
{
    public class Response
    {
        public byte Status { get; set; }
        public ItemEstimateCollection Collection { get; set; }
    }
}
