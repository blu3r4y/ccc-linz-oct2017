namespace CCC
{
    public class Request
    {
        public string TransactionId { get; set; }
        public string FromOwner { get; set; }
        public string ToOwner { get; set; }
        public int Amount { get; set; }
        public long Timestamp { get; set; }

        public Request(string transactionId, string fromOwner, string toOwner, int amount, long timestamp)
        {
            TransactionId = transactionId;
            FromOwner = fromOwner;
            ToOwner = toOwner;
            Amount = amount;
            Timestamp = timestamp;
        }
    }
}