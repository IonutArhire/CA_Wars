using System;
using System.Collections.Generic;
using Services.Models;
using Services.MapGenerationService;
using Services.PlayerResourcesService;
using Services.RuleSetService;

namespace Services.MatchResourcesService
{
    public class MatchResourcesService: IMatchResourcesService
    {
        private IMapGenerationService _mapGenerationService;

        private IPlayerResourcesService _playerResourcesService;

        private IRuleSetService _ruleSetService;

        public MatchResourcesService(IMapGenerationService mapGenerationService,
                                    IPlayerResourcesService playerResourcesService,
                                    IRuleSetService ruleSetService) {
            
            this._mapGenerationService = mapGenerationService;
            this._playerResourcesService = playerResourcesService;
            this._ruleSetService = ruleSetService;
        }

        public MatchModel GetMatchResources(DimensionsModel dimensions, int nrPlayers, int maxGenerations, string ruleset) {
            var playerResources = this._playerResourcesService.GetPlayerResources(nrPlayers);
            var playerNumbers = this._playerResourcesService.InitPlayerNumbers(nrPlayers);
            var gameMap = this._mapGenerationService.RandomGen(dimensions, nrPlayers);
            var ruleSet = this._ruleSetService.GetByName(ruleset);
            var result = new MatchModel(dimensions, maxGenerations, playerResources, playerNumbers, gameMap, ruleSet);

            return result;
        }
    }
}