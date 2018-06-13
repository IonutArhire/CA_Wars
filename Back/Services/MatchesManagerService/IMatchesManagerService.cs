using System;
using Services.Models;

namespace Services.MatchesManagerService
{
    public interface IMatchesManagerService
    {
        void Create(Guid gameKey, GameModel gameModel);

        void RegisterPlayer(string connectionId, int assignedNumber, Guid gameKey);

        void UnRegisterPlayer(string connectionId);

        GameModel GetGameModel(Guid gameKey);
    }
}