using Microsoft.AspNetCore.SignalR;

namespace myfancyproj.Messages
{
    public class PaymentRequest
    {
        public PaymentRequest(string siteName, 
            PaymentInformation payment, 
            CustomerInformation customer,
            HubCallerContext hubContext)
        {
            SiteName = siteName;
            Payment = payment;
            Customer = customer;
            HubContext = hubContext;
        }

        public string SiteName { get; }

        public PaymentInformation Payment { get; }
        public CustomerInformation Customer { get; }
        public HubCallerContext HubContext { get; }
    }
}