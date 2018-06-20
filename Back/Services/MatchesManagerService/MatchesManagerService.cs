using System;
using System.Collections.Concurrent;
using Services.Models;

namespace Services.MatchesManagerService
{
    public class MatchesManagerService: IMatchesManagerService
    {
        private ConcurrentDictionary<Guid, GameModel> _matches;

        private ConcurrentDictionary<string, GameModel> _connections;

        private int FindPlayerId(string connectionId, GameModel game) {
            int result = -1;
            for (int i = 0; i < game.Players.Count; i++)
            {
                if (game.Players[i].ConnectionId == connectionId) {
                    result = i;
                    break;
                }
            }
            return result;
        }
        
        public MatchesManagerService() {
            this._matches = new ConcurrentDictionary<Guid, GameModel>();
            this._connections = new ConcurrentDictionary<string, GameModel>(System.StringComparer.OrdinalIgnoreCase);
        }

        public void Create(Guid gameKey, GameModel gameModel) {
            this._matches.TryAdd(gameKey, gameModel);
        }

        public void RegisterPlayer(string connectionId, int assignedNumber, Guid gameKey) {
            var game = this._matches[gameKey];
            this._connections.TryAdd(connectionId, game);

            game.Players[assignedNumber].ConnectionId = connectionId;
        }

        public void UnRegisterPlayer(string connectionId) {
            var game = this._connections[connectionId];

            var playerId = this.FindPlayerId(connectionId, game);
            game.Players.RemoveAt(playerId);
        }

        public GameModel GetGameModel(Guid gameKey) {
            return this._matches[gameKey];
        }

        public bool GameModelExists(Guid gameKey) {
            return this._matches.ContainsKey(gameKey);
        }
    }
}