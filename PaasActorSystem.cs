using Akka.Actor;
using Microsoft.Extensions.DependencyInjection;
using myfancyproj.Actors;
using web4fancyproj.SignalR;

namespace web4fancyproj
{
    public static class PaasActorSystem
    {
        private static ActorSystem _paasActorSystem;

        public static IActorRef Paas { get; private set; }

        public static IActorRef SignalRBridge { get; private set; }

        public static void Create(ServiceProvider provider)
        {
            var eventPusher = provider.GetService<IEventPusher>();

            _paasActorSystem = ActorSystem.Create("Paas");
            Paas = _paasActorSystem.ActorOf(Props.Create(() => new PaasActor()));
            SignalRBridge = _paasActorSystem.ActorOf(Props.Create(()=> new SignalRBridgeActor(eventPusher, Paas)), "SignalRBridgeActor");
        }

        public static void Shutdown()
        {
            // _paasActorSystem.Shutdown();
            // _paasActorSystem.WaitTermination();
        }
    }
}