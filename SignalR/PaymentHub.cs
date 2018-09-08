using System;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.AspNetCore.SignalR;
using myfancyproj.Messages;

namespace web4fancyproj.SignalR
{
    public class PaymentHub : Hub
    {
        private PaasActorSystem _actorSystem;
        public PaymentHub(PaasActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
        }

        public void Deposit(int amount)
        {
            var request = new PaymentRequest("test.com", 
                new PaymentInformation("EUR",0,"EUR",0,"EUR",0,"VISA",amount,amount,DateTime.UtcNow,DateTime.UtcNow,Guid.NewGuid().ToString(),string.Empty,0,"EUR",1),
                new CustomerInformation("SE",Guid.NewGuid().ToString()),
                Context.ConnectionId);

            _actorSystem.SignalRBridge.Tell(request);
        }
    }
}