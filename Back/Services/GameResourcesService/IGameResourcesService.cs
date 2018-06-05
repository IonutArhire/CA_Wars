using Services.Models;

namespace Services.GameResourcesService
{
    public interface IGameResourcesService
    {
        GameModel GetGameResources(DimensionsModel dimensions, int nrPlayers, int maxGenerations);
    }
}