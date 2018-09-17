using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Services.Models;
using Services.MatchResourcesService;
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
        private IMatchResourcesService _matchResourcesService;

        private IPlayerResourcesService _playerResourcesService;

        private IAlgorithmService _algorithmService;

        private IMatchesManagerService _matchesManagerService;

        private IMapper _mapper;

        private IHostingEnvironment _env;

        public MatchHub(IMatchResourcesService matchResourcesService,
                        IPlayerResourcesService playerResourcesService,
                        IAlgorithmService algorithmService,
                        IMatchesManagerService matchesManagerService,
                        IMapper mapper,
                        IHostingEnvironment env) {

            this._matchResourcesService = matchResourcesService;
            this._playerResourcesService = playerResourcesService;
            this._algorithmService = algorithmService;
            this._matchesManagerService = matchesManagerService;
            this._mapper = mapper;
            this._env = env;
        }

        private void TestingInitializations() {
            var test_gameModel1 = this._matchResourcesService.GetMatchResources(new DimensionsModel(15, 30), 2, 100, "GOL");
            var test_gameModel2 = this._matchResourcesService.GetMatchResources(new DimensionsModel(15, 30), 2, 100, "Coagulations");
            var test_gameModel3 = this._matchResourcesService.GetMatchResources(new DimensionsModel(60, 90), 4, 150, "Coagulations");

            var id1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var id2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var id3 = Guid.Parse("00000000-0000-0000-0000-000000000003");

            this._matchesManagerService.Create(id1, test_gameModel1);
            this._matchesManagerService.Create(id2, test_gameModel2);
            this._matchesManagerService.Create(id3, test_gameModel3);
        }

        public override async Task OnConnectedAsync() {
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

        public async Task SendResources(string unparsedMatchKey) {
            var matchKey = Guid.Parse(unparsedMatchKey);
                
            if (this._matchesManagerService.GameModelExists(matchKey)) {
                var match = this._matchesManagerService.GetMatchModel(matchKey);

                if (match.PlayerNumbers.Count == 0) {
                    throw new HubException($"The match with the key \"{unparsedMatchKey}\" is already full!");
                }

                var matchModelDto = this._mapper.Map<MatchModelDto>(match);

                matchModelDto.AssignedNumber = this._playerResourcesService.AssignNumber(match);
                matchModelDto.Map = this._playerResourcesService.GetPersonalizedMap(match.Map, matchModelDto.AssignedNumber);

                await Clients.Caller.SendAsync("Resources", matchModelDto);
                await Groups.AddToGroupAsync(Context.ConnectionId, matchKey.ToString());
                this._matchesManagerService.RegisterPlayer(Context.ConnectionId, matchModelDto.AssignedNumber, matchKey);
            }
            else {
                throw new HubException($"The match with the key \"{unparsedMatchKey}\" is non-existent!");
            }
        }

        public async Task SendConfig(string unparsedMatchKey, float[,] playerConfig, int assignedNumber) {
            var matchKey = Guid.Parse(unparsedMatchKey);

            var match = this._matchesManagerService.GetMatchModel(matchKey);
            var playerId = this._matchesManagerService.FindPlayerId(Context.ConnectionId, match);

            match.InitialConfigs.Add(new InitialConfigModel(playerConfig, playerId));

            await Clients.Group(matchKey.ToString()).SendAsync("PlayerSent", assignedNumber);

            if (match.InitialConfigs.Count == match.Players.Count) {
                var result = this._algorithmService.RunGame(match);

                await Clients.Group(matchKey.ToString()).SendAsync("Game", result);
                match.InitialConfigs.Clear();
            }
        }
    }
}