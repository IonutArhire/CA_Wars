using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Services.Models;
using Services.GameResourcesService;
using Services.AlgorithmService;
using Services.PlayerResourcesService;
using Services.MatchesManagerService;
using Api.Dtos;
using AutoMapper;

namespace Api.Hubs {
    public class MatchHub : Hub 
    {
        private IGameResourcesService _gameResourcesService;

        private IPlayerResourcesService _playerResourcesService;

        private IAlgorithmService _algorithmService;

        private IMatchesManagerService _matchesManagerService;

        private IMapper _mapper;

        public MatchHub(IGameResourcesService gameResourcesService,
                        IPlayerResourcesService playerResourcesService,
                        IAlgorithmService algorithmService,
                        IMatchesManagerService matchesManagerService,
                        IMapper mapper) {

            this._gameResourcesService = gameResourcesService;
            this._playerResourcesService = playerResourcesService;
            this._algorithmService = algorithmService;
            this._matchesManagerService = matchesManagerService;
            this._mapper = mapper;
        }

        private void TestingInitializations() {
            var test_gameModel1 = _gameResourcesService.GetGameResources(new DimensionsModel(15, 30), 2, 10);
            var test_gameModel2 = _gameResourcesService.GetGameResources(new DimensionsModel(5, 15), 2, 100);

            this._matchesManagerService.Create("1", test_gameModel1);
            this._matchesManagerService.Create("2", test_gameModel2);
        }

        public override async Task OnConnectedAsync() {
            TestingInitializations();
            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(System.Exception ex) {
            await Clients.All.SendAsync("Disconnected", Context.ConnectionId);
        }

        public async Task SendResources(string gameKey) {
            var game = this._matchesManagerService.GetGameModel(gameKey);
            var gameModelDto = this._mapper.Map<GameModelDto>(game);

            var assignedNumber = this._playerResourcesService.AssignNumber(game);
            gameModelDto.AssignedNumber = assignedNumber;
            var personalizedMap = this._playerResourcesService.GetPersonalizedMap(game.Map, assignedNumber);
            gameModelDto.Map = personalizedMap;

            await Clients.Caller.SendAsync("Resources", gameModelDto);
        }

        public async Task SendConfig(string gameKey, float[,] playerConfig) {
            var game = this._matchesManagerService.GetGameModel(gameKey);
            game.InitialConfigs.Add(playerConfig);

            if (game.InitialConfigs.Count == game.NrPlayers) {
                var result = this._algorithmService.RunGame(game);
                
                await Clients.All.SendAsync("Game", result);
                game.InitialConfigs.Clear();
            }
        }
    }
}