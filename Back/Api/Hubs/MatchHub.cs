using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Services.PlayerResourcesService;

namespace Api.Hubs
{
    public class MatchHub : Hub
    {
        private static List<float[][]> configurations = new List<float[][]>();

        private static ConcurrentDictionary<string, int> _players = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public override async Task OnConnectedAsync()
        {
            var resources = PlayerResourcesService.getPlayerResources();
            _players.TryAdd(Context.ConnectionId, Convert.ToInt32(resources.GetValueOrDefault("number")));
            await Clients.All.SendAsync("SendConnected", resources);
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("SendDisconnected", Context.ConnectionId);
        }

        public void Send(float[][] matrice)
        {
            configurations.Add(matrice);
        }
    }
}