using Services.Models;

namespace Services.MatchesManagerService
{
    public interface IMatchesManagerService
    {
        void Create(string gameKey, GameModel gameModel);

        GameModel GetGameModel(string gameKey);
    }
}