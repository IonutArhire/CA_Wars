using Services.Models;

namespace Services.MatchesManagerService
{
    public interface IMatchesManagerService
    {
        void Create(string gameKey, GameModel gameModel);

        void RegisterPlayer(string connectionId, int assignedNumber, string gameKey);

        void UnRegisterPlayer(string connectionId);

        GameModel GetGameModel(string gameKey);
    }
}