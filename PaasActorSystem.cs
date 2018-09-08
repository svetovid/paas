using System;
using Akka.Actor;
using Akka.DI.Core;
using Microsoft.AspNetCore.SignalR;
using myfancyproj.Actors;
using web4fancyproj.SignalR;

namespace web4fancyproj
{
    public class PaasActorSystem
    {
        private readonly IHubContext<PaymentHub> _hubContext;

        public PaasActorSystem(IHubContext<PaymentHub> hubContext)
        {
            _hubContext = hubContext;

            Create();
        }

        private ActorSystem _paasActorSystem;

        private IActorRef Paas { get; set; }

        public IActorRef SignalRBridge { get; private set; }

        private void Create()
        {
            _paasActorSystem = ActorSystem.Create("Paas");
            //Paas = _paasActorSystem.ActorOf(_paasActorSystem.DI().Props<PaasActor>());
            Paas = _paasActorSystem.ActorOf(Props.Create(() => new PaasActor()));
            SignalRBridge = _paasActorSystem.ActorOf(Props.Create(()=> new SignalRBridgeActor(_hubContext, Paas)), "SignalRBridgeActor");
            //SignalRBridge = _paasActorSystem.ActorOf(_paasActorSystem.DI().Props<SignalRBridgeActor>(), "SignalRBridgeActor");
        }

        public static void Shutdown()
        {
            // _paasActorSystem.Shutdown();
            // _paasActorSystem.WaitTermination();
        }
    }
}