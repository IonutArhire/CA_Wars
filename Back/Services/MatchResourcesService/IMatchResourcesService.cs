using Services.Models;

namespace Services.MatchResourcesService
{
    public interface IMatchResourcesService
    {
        MatchModel GetMatchResources(DimensionsModel dimensions, int nrPlayers, int maxGenerations, string ruleset);
    }
}