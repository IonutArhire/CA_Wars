using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Services.AlgorithmService;
using Services.GameResourcesService;
using Services.PlayerResourcesService;

namespace Api.Hubs {
    public class MatchHub : Hub {
        private static List<float[][]> configurations = new List<float[][]> ();
        private static int size = 30;

        private static ConcurrentDictionary<string, int> _players = new ConcurrentDictionary<string, int> (StringComparer.OrdinalIgnoreCase);

        public override async Task OnConnectedAsync() {
            var resources = GameResourcesService.getGameResources();
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
                var result = new { generations = generations, winner = AlgorithmService.getWinner() };
                await Clients.All.SendAsync("SendGame", result);
                configurations.Clear();
            }
        }

    }
}