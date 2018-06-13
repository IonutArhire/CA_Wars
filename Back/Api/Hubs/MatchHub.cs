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
using System;
using Microsoft.AspNetCore.Hosting;

namespace Api.Hubs {
    public class MatchHub : Hub 
    {
        private IGameResourcesService _gameResourcesService;

        private IPlayerResourcesService _playerResourcesService;

        private IAlgorithmService _algorithmService;

        private IMatchesManagerService _matchesManagerService;

        private IMapper _mapper;

        private IHostingEnvironment _env;

        public MatchHub(IGameResourcesService gameResourcesService,
                        IPlayerResourcesService playerResourcesService,
                        IAlgorithmService algorithmService,
                        IMatchesManagerService matchesManagerService,
                        IMapper mapper,
                        IHostingEnvironment env) {

            this._gameResourcesService = gameResourcesService;
            this._playerResourcesService = playerResourcesService;
            this._algorithmService = algorithmService;
            this._matchesManagerService = matchesManagerService;
            this._mapper = mapper;
            this._env = env;
        }

        private void TestingInitializations() {
            var test_gameModel1 = this._gameResourcesService.GetGameResources(new DimensionsModel(15, 30), 2, 100, "GOF");
            var test_gameModel2 = this._gameResourcesService.GetGameResources(new DimensionsModel(15, 30), 2, 100, "Coagulations");
            var test_gameModel3 = this._gameResourcesService.GetGameResources(new DimensionsModel(60, 90), 4, 150, "Coagulations");

            var id1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var id2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var id3 = Guid.Parse("00000000-0000-0000-0000-000000000003");

            this._matchesManagerService.Create(id1, test_gameModel1);
            this._matchesManagerService.Create(id2, test_gameModel2);
            this._matchesManagerService.Create(id3, test_gameModel3);
        }

        public override async Task OnConnectedAsync() {
            TestingInitializations();
            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(System.Exception ex) {
            await Clients.All.SendAsync("Disconnected", Context.ConnectionId);

            if (this._env.IsDevelopment()) {

            }
            else {
                this._matchesManagerService.UnRegisterPlayer(Context.ConnectionId);
            }
        }

        public async Task SendResources(string gameKeyUnparsed) {
            var gameKey = Guid.Parse(gameKeyUnparsed);
            var game = this._matchesManagerService.GetGameModel(gameKey);
            var gameModelDto = this._mapper.Map<GameModelDto>(game);

            var assignedNumber = this._playerResourcesService.AssignNumber(game);
            gameModelDto.AssignedNumber = assignedNumber;
            var personalizedMap = this._playerResourcesService.GetPersonalizedMap(game.Map, assignedNumber);
            gameModelDto.Map = personalizedMap;

            await Clients.Caller.SendAsync("Resources", gameModelDto);
            await Groups.AddToGroupAsync(Context.ConnectionId, gameKey.ToString());
            this._matchesManagerService.RegisterPlayer(Context.ConnectionId, assignedNumber, gameKey);
        }

        public async Task SendConfig(Guid gameKey, float[,] playerConfig) {
            var game = this._matchesManagerService.GetGameModel(gameKey);
            game.InitialConfigs.Add(playerConfig);

            if (game.InitialConfigs.Count == game.Players.Count) {
                var result = this._algorithmService.RunGame(game);

                await Clients.Group(gameKey.ToString()).SendAsync("Game", result);
                game.InitialConfigs.Clear();
            }
        }
    }
}