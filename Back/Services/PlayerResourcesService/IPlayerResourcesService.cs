using System.Collections.Generic;
using Services.Models;

namespace Services.PlayerResourcesService
{
    public interface IPlayerResourcesService
    {
        List<string> InitializeColors();

        List<int> InitPlayerNumbers(int nrPlayers);

        int AssignNumber(GameModel game);

        List<PlayerModel> GetPlayerResources(int nrPlayers);

        float[,] GetPersonalizedMap(float[,] map, int assignedNumber);
    }
}