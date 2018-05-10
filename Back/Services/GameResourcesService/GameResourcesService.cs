using System;
using System.Collections.Generic;
using Services.Models;
using static Services.PlayerResourcesService.PlayerResourcesService;

namespace Services.GameResourcesService
{
    public static class GameResourcesService
    {
        public static GameModel getGameResources(int size, int nrPlayers, int maxGenerations) {
            var playerResources = getPlayerResources();
            var result = new GameModel(size, nrPlayers, maxGenerations, playerResources, new List<float[][]>());

            return result;
        }
    }
}