namespace myfancyproj.Messages
{
    public class PspResponse
    {
        public PspResponse(string payref, string status, string publicPaymentId)
        {
            PaymentReference = payref;
            Status = status;
            PublicPaymentId = publicPaymentId;
        }

        public string PaymentReference { get; }

        public string Status { get; }
        public string PublicPaymentId { get; }
    }
}