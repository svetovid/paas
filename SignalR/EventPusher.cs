using System;
using Microsoft.AspNetCore.SignalR;

namespace web4fancyproj.SignalR 
{
    public class EventPusher : IEventPusher
    {
        private IHubContext<EchoHub> _hubContext;

        public EventPusher(IHubContext<EchoHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void UpdateStatus(string connectionId, string status)
        {
            Console.WriteLine("Event pusher - payment updated: {0}", status);
            
            _hubContext.Clients.Client(connectionId).SendAsync("UpdateStatus", status);
        }
    }
}