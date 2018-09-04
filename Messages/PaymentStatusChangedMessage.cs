namespace myfancyproj.Messages
{
    public class PaymentStatusChangedMessage
    {
        public PaymentStatusChangedMessage(string siteName, 
            PaymentInformation payment,
            CustomerInformation customer)
        {
            SiteName = siteName;
            Payment = payment;
            Customer = customer;
        }

        public string SiteName { get; }
        public PaymentInformation Payment { get; }
        public CustomerInformation Customer { get; }
        public string Status { get; }
    }
}