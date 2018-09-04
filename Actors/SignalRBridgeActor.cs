using System;
using Akka.Actor;
using myfancyproj.Messages;
using web4fancyproj.SignalR;

namespace myfancyproj.Actors
{
    public class SignalRBridgeActor : ReceiveActor
    {
        private string _connectionId;

        public SignalRBridgeActor(IEventPusher eventPusher, IActorRef paasActor)
        {
            Receive<PaymentRequest>(msg => {
                Console.WriteLine("Connection id: {0}", msg.HubContext.ConnectionId);
                _connectionId = msg.HubContext.ConnectionId;
                
                Console.WriteLine("SignalRBridgeActor");
                paasActor.Tell(msg);
            });

            Receive<PaymentStatus>(msg => {
                Console.WriteLine("Payment status connection id: {0}", _connectionId);

                eventPusher.UpdateStatus(_connectionId, msg.Status);
                Console.WriteLine("SignalRBridgeActor Status changed {0}: {1}", msg.PaymentReference, msg.Status);
            });
        }
    }
}