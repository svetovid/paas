namespace web4fancyproj.SignalR
{
    public interface IEventPusher
    {
        void UpdateStatus(string connectionId, string status);
    }
}