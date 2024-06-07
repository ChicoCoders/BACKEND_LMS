using Microsoft.AspNet.SignalR;

namespace LMS.Hubs
{
    public class YHub: Hub
    {
        public async Task JoinRoom(string roomName)
        {
            await Groups.Add(Context.ConnectionId, roomName);
        }
        public async Task LeaveRoom(string roomName)
        {
            await Groups.Remove(Context.ConnectionId, roomName);
        }

    }
}
