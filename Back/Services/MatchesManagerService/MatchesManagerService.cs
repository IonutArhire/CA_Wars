using System;
using System.Collections.Concurrent;
using Services.Models;

namespace Services.MatchesManagerService
{
    public class MatchesManagerService: IMatchesManagerService
    {
        private ConcurrentDictionary<Guid, MatchModel> _matches;

        private ConcurrentDictionary<string, MatchModel> _connections;

        private int FindPlayerId(string connectionId, MatchModel match) {
            int result = -1;
            for (int i = 0; i < match.Players.Count; i++)
            {
                if (match.Players[i].ConnectionId == connectionId) {
                    result = i;
                    break;
                }
            }
            return result;
        }

        private Guid GetMatchKey(MatchModel match) {
            foreach (var elem in this._matches)
            {
                if (elem.Value == match) {
                    return elem.Key;
                }
            }

            return Guid.Empty;
        }

        private void RemoveMatchConnections(MatchModel match) {
            foreach (var cnx in this._connections)
            {
                if (cnx.Value == match) {
                    MatchModel redundant;
                    this._connections.TryRemove(cnx.Key, out redundant);
                }
            }
        }

        private void RemoveMatch(MatchModel match) {
            RemoveMatchConnections(match);

            var matchKey = GetMatchKey(match);
            MatchModel redundant;
            this._matches.TryRemove(matchKey, out redundant);
        }
        
        public MatchesManagerService() {
            this._matches = new ConcurrentDictionary<Guid, MatchModel>();
            this._connections = new ConcurrentDictionary<string, MatchModel>(System.StringComparer.OrdinalIgnoreCase);
        }

        public void Create(Guid matchKey, MatchModel gameModel) {
            this._matches.TryAdd(matchKey, gameModel);
        }

        public void RegisterPlayer(string connectionId, int assignedNumber, Guid matchKey) {
            var match = this._matches[matchKey];
            this._connections.TryAdd(connectionId, match);

            match.Players[assignedNumber].ConnectionId = connectionId;
        }

        public void UnRegisterPlayer(string connectionId) {
            var match = this._connections[connectionId];

            var playerId = this.FindPlayerId(connectionId, match);
            match.Players.RemoveAt(playerId);

            MatchModel redundant;
            this._connections.TryRemove(connectionId, out redundant);

            if (match.Players.Count == 0) {
                RemoveMatch(match);
            }
        }

        public MatchModel GetMatchModel(Guid matchKey) {
            return this._matches[matchKey];
        }

        public bool GameModelExists(Guid matchKey) {
            return this._matches.ContainsKey(matchKey);
        }
    }
}