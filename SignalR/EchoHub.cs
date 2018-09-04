using System;
using Akka.Actor;
using Microsoft.AspNetCore.SignalR;
using myfancyproj.Messages;

namespace web4fancyproj.SignalR
{
    public class EchoHub : Hub
    {
        public void Echo(string message)
        {
            Clients.All.SendAsync("Send", message);
        }

        public void Deposit(int amount)
        {
            var request = new PaymentRequest("test.com", 
                new PaymentInformation("EUR",0,"EUR",0,"EUR",0,"VISA",amount,amount,DateTime.UtcNow,DateTime.UtcNow,Guid.NewGuid().ToString(),string.Empty,0,"EUR",1),
                new CustomerInformation("SE",Guid.NewGuid().ToString()),
                Context);
            
            PaasActorSystem.SignalRBridge.Tell(request);
        }
    }
}