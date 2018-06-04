using System;
using System.Collections.Generic;
using Services.Models;
using static Services.PlayerResourcesService.PlayerResourcesService;
using static Services.MapGenerationService.MapGenerationService;

namespace Services.GameResourcesService
{
    public static class GameResourcesService
    {
        public static GameModel GetGameResources(DimensionsModel dimensions, int nrPlayers, int maxGenerations) {
            var playerResources = GetPlayerResources(nrPlayers);
            var playerNumbers = InitPlayerNumbers(nrPlayers);
            var gameMap = RandomGen(dimensions, nrPlayers);
            var result = new GameModel(dimensions, nrPlayers, maxGenerations, playerResources, playerNumbers, gameMap);

            return result;
        }
    }
}