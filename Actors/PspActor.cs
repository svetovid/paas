using System;
using Akka.Actor;
using myfancyproj.Messages;

namespace myfancyproj.Actors
{
    public class PspActor : ReceiveActor
    {
        public PspActor()
        {
            Receive<PaymentRequest>(msg => {
                Console.WriteLine("PspActor");
                System.Threading.Thread.Sleep(2000);
                //ActorRef.Tell(message, Sender)
                var status = msg.Payment.PaymentAmount > 100 ? "Refused" : "Captured";
                Sender.Tell(new PspResponse(msg.Payment.PaymentReference, status, "qwertyy123"));
            });
        }
    }
}