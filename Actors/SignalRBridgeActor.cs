using System;
using Akka.Actor;
using Microsoft.AspNetCore.SignalR;
using myfancyproj.Messages;
using web4fancyproj.SignalR;

namespace myfancyproj.Actors
{
    public class SignalRBridgeActor : ReceiveActor
    {
        private string _connectionId;

        public SignalRBridgeActor(IHubContext<PaymentHub> hubContext, IActorRef paasActor)
        {
            Receive<PaymentRequest>(msg => {
                Console.WriteLine("Connection id: {0}", msg.ConnectionId);
                _connectionId = msg.ConnectionId;
                
                Console.WriteLine("SignalRBridgeActor");
                paasActor.Tell(msg);
            });

            Receive<PaymentStatus>(msg => {
                Console.WriteLine("Payment status connection id: {0}", _connectionId);

                hubContext.Clients.All.SendAsync("UpdateStatus", msg.Status);
                Console.WriteLine("SignalRBridgeActor Status changed {0}: {1}", msg.PaymentReference, msg.Status);
            });
        }
    }
}