using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;

namespace LoiNguyen.Samples.SignalR6.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            if (string.IsNullOrEmpty(user))
                await Clients.All.SendAsync("ReceiveMessageHandler", message);
            else
                await Clients.User(user).SendAsync("ReceiveMessageHandler", message);
        }

        public async Task BroadcastStream(IAsyncEnumerable<string> stream)
        {
            await foreach (var item in stream)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", $"Server received {item}");
            }
        }

        public async IAsyncEnumerable<string> TriggerStream(int jobsCount, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            for (var i = 0; i < jobsCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return $"Job {i} executed successfully";
                await Task.Delay(1000, cancellationToken);
            }
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "HubUsers");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "HubUsers");
            await base.OnDisconnectedAsync(exception);
        }

    }
}