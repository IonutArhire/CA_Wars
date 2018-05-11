using System;
using System.Collections.Generic;
using Services.Models;
using static Services.PlayerResourcesService.PlayerResourcesService;

namespace Services.GameResourcesService
{
    public static class GameResourcesService
    {
        public static GameModel GetGameResources(int size, int nrPlayers, int maxGenerations) {
            var playerResources = GetPlayerResources(nrPlayers);
            var playerNumbers = InitPlayerNumbers(nrPlayers);
            var result = new GameModel(size, nrPlayers, maxGenerations, playerResources, new List<float[][]>(), playerNumbers);

            return result;
        }
    }
}