using System;
using System.Collections.Generic;
using Akka.Actor;
using myfancyproj.Messages;

namespace myfancyproj.Actors
{
    public class PaasActor : ReceiveActor
    {
        private IActorRef _validationActor;

        private IActorRef _pspActor;

        private Dictionary<string, IActorRef> _payments = new Dictionary<string, IActorRef>();

        public PaasActor()
        {
            _validationActor = Context.ActorOf(Props.Create(() => new ValidationActor()), "validator");
            _pspActor = Context.ActorOf(Props.Create(() => new PspActor()), "psp");

            Receive<PaymentRequest>(msg => {
                if (!_payments.ContainsKey(msg.Payment.PaymentReference))
                {
                    var paymentActor = Context.ActorOf(Props.Create(() => new PaymentActor(msg.Payment.PaymentReference, Sender,_validationActor, _pspActor)));
                    paymentActor.Tell(msg);

                    _payments.Add(msg.Payment.PaymentReference, paymentActor);
                }

                Console.WriteLine("Payment is already being processed.");
            });

            // Receive<PaymentStatus>(msg => {
            //     Console.WriteLine("PaasActor {0}: {1}.", msg.PaymentReference, msg.Status);

            //     Console.WriteLine("{0}", Sender.Path);
            //     //_payments.Remove(msg.PaymentReference);
            //     Sender.Tell(msg);
            // });
        }
    }
}