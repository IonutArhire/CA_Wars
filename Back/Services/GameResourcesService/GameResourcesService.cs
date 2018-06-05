using System;
using System.Collections.Generic;
using Services.Models;
using Services.MapGenerationService;
using Services.PlayerResourcesService;

namespace Services.GameResourcesService
{
    public class GameResourcesService: IGameResourcesService
    {
        private IMapGenerationService _mapGenerationService;

        private IPlayerResourcesService _playerResourcesService;

        public GameResourcesService(IMapGenerationService mapGenerationService,
                                    IPlayerResourcesService playerResourcesService) {
            
            this._mapGenerationService = mapGenerationService;
            this._playerResourcesService = playerResourcesService;
        }

        public GameModel GetGameResources(DimensionsModel dimensions, int nrPlayers, int maxGenerations) {
            var playerResources = this._playerResourcesService.GetPlayerResources(nrPlayers);
            var playerNumbers = this._playerResourcesService.InitPlayerNumbers(nrPlayers);
            var gameMap = this._mapGenerationService.RandomGen(dimensions, nrPlayers);
            var result = new GameModel(dimensions, nrPlayers, maxGenerations, playerResources, playerNumbers, gameMap);

            return result;
        }
    }
}