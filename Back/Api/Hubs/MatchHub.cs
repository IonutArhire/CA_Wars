using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Services.Models;
using Services.GameResourcesService;
using Services.AlgorithmService;
using Services.PlayerResourcesService;

namespace Api.Hubs {
    public class MatchHub : Hub 
    {
        private static ConcurrentDictionary<string, GameModel> _games = new ConcurrentDictionary<string, GameModel> (StringComparer.OrdinalIgnoreCase);

        private void TestingInitializations() {
            var test_gameModel = GameResourcesService.getGameResources(20, 2, 100);

            _games.AddOrUpdate("1", test_gameModel, (key, gamemodel) => { return gamemodel;});
        }

        public override async Task OnConnectedAsync() {
            TestingInitializations();
            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception ex) {
            await Clients.All.SendAsync("Disconnected", Context.ConnectionId);
        }

        public async Task SendResources(string gameKey) {
            var game = _games[gameKey];
            var assignedNumber = PlayerResourcesService.getNewNumber();
            var resources = new ResourcesModel(game, assignedNumber);

            await Clients.Caller.SendAsync("Resources", resources);
        }

        public async Task SendInputConfig(string gameKey, float[][] playerConfig) {
            var game = _games[gameKey];
            game.Configs.Add(playerConfig);

            if (game.Configs.Count == game.NrPlayers) {
                var counter = 0;
                
                List<float[][]> generations = new List<float[][]>();
                generations.Add(AlgorithmService.Initialize(game.Configs, game.Size));
                while (!AlgorithmService.isGridEmpty() && counter != game.MaxGenerations) {
                    generations.Add(AlgorithmService.RunNextGen());
                    counter++;
                }

                var result = new GameResultModel(generations, AlgorithmService.getWinner());
                
                await Clients.All.SendAsync("Game", result);
                game.Configs.Clear();
            }
        }
    }
}