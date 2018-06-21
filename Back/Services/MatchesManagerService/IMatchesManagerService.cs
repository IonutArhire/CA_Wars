using System;
using Services.Models;

namespace Services.MatchesManagerService
{
    public interface IMatchesManagerService
    {
        void Create(Guid matchKey, MatchModel gameModel);

        void RegisterPlayer(string connectionId, int assignedNumber, Guid matchKey);

        void UnRegisterPlayer(string connectionId);

        MatchModel GetMatchModel(Guid matchKey);

        bool GameModelExists(Guid matchKey);
    }
}