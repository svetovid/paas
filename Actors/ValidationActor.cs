using System;
using Akka.Actor;
using myfancyproj.Messages;

namespace myfancyproj.Actors
{
    public class ValidationActor : ReceiveActor
    {
        public ValidationActor()
        {
            Receive<PaymentRequest>(msg => {
                Console.WriteLine("ValidationActor");
                System.Threading.Thread.Sleep(2000);
                //FraudActor.Tell
                //IrActor.Tell
                //ClosedLoopActor.Tell
                //KycActor.Tell

                int code = 0;
                string statusMessage = "";

                if (msg.Payment.PaymentAmount == 42)
                {
                    code = 10005;
                    statusMessage = "Magic number";
                }

                Sender.Tell(new ValidationResponse(code, statusMessage));
            });
        }
    }
}