using Microsoft.AspNetCore.SignalR;

namespace ParkingSpaces.Hubs
{
    public class ParkSpaceAvailabilityHub : Hub
    {
        // testing purposes
        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine(message);
            // Broadcast the message to all clients
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
