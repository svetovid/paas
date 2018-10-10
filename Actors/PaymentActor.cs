using System;
using Akka.Actor;
using Akka.Persistence;
using myfancyproj.Messages;

namespace myfancyproj.Actors
{
    public class PaymentActor : ReceiveActor
    {
        private Payment _payment;

        private IActorRef _validationActor;

        private IActorRef _pspActor;

        private IActorRef _sender;

        //public override string PersistenceId => _payment.PaymentReference;

        public PaymentActor(string paymentReference, IActorRef sender, IActorRef validationActor, IActorRef pspActor)
        {
            _sender = sender;
            _validationActor = validationActor;
            _pspActor = pspActor;

            _payment = new Payment();

            Become(Initiated);
        }

        private void Initiated()
        {
            Console.WriteLine("PaymentActor before Initiated");
            Receive<PaymentRequest>(msg =>
            {
                Console.WriteLine("PaymentActor Initiated {0}", msg.Payment.PaymentReference);
                _payment.PaymentReference = msg.Payment.PaymentReference;
                _payment.CustomerReference = msg.Customer.CustomerReference;
                _payment.Amount = msg.Payment.PaymentAmount;
                _payment.CurrencyCode = msg.Payment.CurrencyCode;
                _payment.MethodActionId = msg.Payment.MethodActionId;

                //ValidationActor tell payment request
                _validationActor.Tell(msg);

                Become(Created);
            });
        }

        private void Created()
        {
            Receive<ValidationResponse>(msg => {
                Console.WriteLine("PaymentActor Created");
                if(msg.StatusCode == 0) {
                    _payment.ProviderAccountName = msg.StatusMessage;
                    _sender.Tell(new PaymentStatus(_payment.PaymentReference, "Created"));

                    //PSPActor tell payment request
                    var request = new PaymentRequest("test.com",
                        new PaymentInformation("EUR",0,"EUR",0,"EUR",0,"VISA",1,2,DateTime.UtcNow,DateTime.UtcNow,string.Empty,string.Empty,0,"EUR",1),
                        new CustomerInformation("SE", Guid.NewGuid().ToString()),
                        null);
                    _pspActor.Tell(request);

                    Become(Pending);
                }
                else
                {
                    _sender.Tell(new PaymentStatus(_payment.PaymentReference, "Invalid"));
                }
            });
        }

        private void Pending()
        {
            _sender.Tell(new PaymentStatus(_payment.PaymentReference, "InProcess"));

            Receive<PspResponse>(msg => {

                Console.WriteLine("PaymentActor Pending");
                _payment.PublicPaymentId = msg.PublicPaymentId;
                _payment.Status = msg.Status;

                //Send notification to Wallet in order to update customer balance
                Become(Final);
            });
        }

        private void Final()
        {
            Console.WriteLine("PaymentActor Final");
            _sender.Tell(new PaymentStatus(_payment.PaymentReference, _payment.Status));

            Self.GracefulStop(TimeSpan.FromSeconds(20));
        }

        //protected override bool ReceiveRecover(object message)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override bool ReceiveCommand(object message)
        //{
        //    throw new NotImplementedException();
        //}
    }
}