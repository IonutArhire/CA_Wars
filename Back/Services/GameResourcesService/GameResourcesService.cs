using System;
using System.Collections.Generic;
using Services.Models;
using Services.MapGenerationService;
using Services.PlayerResourcesService;
using Services.RuleSetService;

namespace Services.GameResourcesService
{
    public class GameResourcesService: IGameResourcesService
    {
        private IMapGenerationService _mapGenerationService;

        private IPlayerResourcesService _playerResourcesService;

        private IRuleSetService _ruleSetService;

        public GameResourcesService(IMapGenerationService mapGenerationService,
                                    IPlayerResourcesService playerResourcesService,
                                    IRuleSetService ruleSetService) {
            
            this._mapGenerationService = mapGenerationService;
            this._playerResourcesService = playerResourcesService;
            this._ruleSetService = ruleSetService;
        }

        public GameModel GetGameResources(DimensionsModel dimensions, int nrPlayers, int maxGenerations, string ruleset) {
            var playerResources = this._playerResourcesService.GetPlayerResources(nrPlayers);
            var playerNumbers = this._playerResourcesService.InitPlayerNumbers(nrPlayers);
            var gameMap = this._mapGenerationService.RandomGen(dimensions, nrPlayers);
            var ruleSet = this._ruleSetService.GetByName(ruleset);
            var result = new GameModel(dimensions, maxGenerations, playerResources, playerNumbers, gameMap, ruleSet);

            return result;
        }
    }
}