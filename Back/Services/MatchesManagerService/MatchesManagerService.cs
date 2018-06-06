using System.Collections.Concurrent;
using Services.Models;

namespace Services.MatchesManagerService
{
    public class MatchesManagerService: IMatchesManagerService
    {
        private ConcurrentDictionary<string, GameModel> _matches;

        private ConcurrentDictionary<string, string> _connections;

        private int FindPlayerById(string connectionId, GameModel game) {
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
            this._matches = new ConcurrentDictionary<string, GameModel>(System.StringComparer.OrdinalIgnoreCase);
            this._connections = new ConcurrentDictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
        }

        public void Create(string gameKey, GameModel gameModel) {
            this._matches.TryAdd(gameKey, gameModel);
        }

        public void RegisterPlayer(string connectionId, int assignedNumber, string gameKey) {
            this._connections.TryAdd(connectionId, gameKey);
            var game = this._matches[gameKey];

            game.Players[assignedNumber].ConnectionId = connectionId;
        }

        public void UnRegisterPlayer(string connectionId) {
            var gameKey = this._connections[connectionId];
            var game = this._matches[gameKey];

            var playerId = this.FindPlayerById(connectionId, game);
            game.Players.RemoveAt(playerId);
        }

        public GameModel GetGameModel(string gameKey) {
            return this._matches[gameKey];
        }
    }
}