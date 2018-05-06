using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Services.AlgorithmService;
using Services.PlayerResourcesService;

namespace Api.Hubs {
    public class MatchHub : Hub {
        private static List<float[][]> configurations = new List<float[][]> ();
        private static int size = 20;

        private static ConcurrentDictionary<string, int> _players = new ConcurrentDictionary<string, int> (StringComparer.OrdinalIgnoreCase);

        public override async Task OnConnectedAsync () {
            var resources = PlayerResourcesService.getPlayerResources ();
            await Clients.Caller.SendAsync("SendConnected", resources);
        }

        public override async Task OnDisconnectedAsync(Exception ex) {
            await Clients.All.SendAsync("SendDisconnected", Context.ConnectionId);
        }

        public async Task Send (float[][] matrix) {
            configurations.Add(matrix);
            var counter = 0;

            if (configurations.Count == 2) {
                List<float[][]> generations = new List<float[][]>();
                generations.Add(AlgorithmService.Initialize(configurations, size));
                while (!AlgorithmService.isGridEmpty() && counter != 500) {
                    generations.Add(AlgorithmService.RunNextGen());
                    counter++;
                }
                await Clients.All.SendAsync("SendGame", generations);
                configurations.Clear();
            }
        }

    }
}