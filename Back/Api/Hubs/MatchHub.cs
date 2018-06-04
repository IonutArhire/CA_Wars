using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Services.Models;
using Services.GameResourcesService;
using Services.AlgorithmService;
using Services.PlayerResourcesService;
using Api.Dtos;

namespace Api.Hubs {
    public class MatchHub : Hub 
    {
        private static ConcurrentDictionary<string, GameModel> _games = new ConcurrentDictionary<string, GameModel> (StringComparer.OrdinalIgnoreCase);

        private void TestingInitializations() {
            var test_gameModel1 = GameResourcesService.GetGameResources(new DimensionsModel(15, 30), 2, 10);
            var test_gameModel2 = GameResourcesService.GetGameResources(new DimensionsModel(5, 15), 2, 100);

            _games.AddOrUpdate("1", test_gameModel1, (key, gamemodel) => { return gamemodel;});
            _games.AddOrUpdate("2", test_gameModel2, (key, gamemodel) => { return gamemodel;});
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
            var assignedNumber = PlayerResourcesService.AssignNumber(game);
            var personalizedMap = PlayerResourcesService.GetPersonalizedMap(game.Map, assignedNumber);
            var resources = new GameModelDto(game.Dimensions, assignedNumber, game.NrPlayers, game.MaxGenerations, game.Players, personalizedMap);

            await Clients.Caller.SendAsync("Resources", resources);
        }

        public async Task SendConfig(string gameKey, float[,] playerConfig) {
            var game = _games[gameKey];
            game.Configs.Add(playerConfig);

            if (game.Configs.Count == game.NrPlayers) {
                var counter = 0;
                
                List<float[,]> generations = new List<float[,]>();
                generations.Add(AlgorithmService.Initialize(game.Configs, game.Dimensions));
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