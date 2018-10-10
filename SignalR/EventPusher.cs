using System;
using Microsoft.AspNetCore.SignalR;

namespace web4fancyproj.SignalR
{
    public class EventPusher : IEventPusher
    {
        private readonly IHubContext<PaymentHub> _hubContext;

        public EventPusher(IHubContext<PaymentHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void UpdateStatus(string connectionId, string status)
        {
            Console.WriteLine("Event pusher - payment updated: {0}", status);

           _hubContext.Clients.All.SendAsync("UpdateStatus", status);
        }
    }
}