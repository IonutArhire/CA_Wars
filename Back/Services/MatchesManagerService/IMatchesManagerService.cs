using System;
using Services.Models;

namespace Services.MatchesManagerService
{
    public interface IMatchesManagerService
    {
        void Create(Guid matchKey, MatchModel matchModel);

        void RegisterPlayer(string connectionId, int assignedNumber, Guid matchKey);

        void UnRegisterPlayer(string connectionId);

        int FindPlayerId(string connectionId, MatchModel match);

        MatchModel GetMatchModel(Guid matchKey);

        bool GameModelExists(Guid matchKey);
    }
}