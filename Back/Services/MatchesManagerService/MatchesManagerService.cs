using System.Collections.Concurrent;
using Services.Models;

namespace Services.MatchesManagerService
{
    public class MatchesManagerService: IMatchesManagerService
    {
        private ConcurrentDictionary<string, GameModel> _matches;
        
        public MatchesManagerService() {
            this._matches = new ConcurrentDictionary<string, GameModel>(System.StringComparer.OrdinalIgnoreCase);
        }

        public void Create(string gameKey, GameModel gameModel) {
            this._matches.TryAdd(gameKey, gameModel);
        }

        public GameModel GetGameModel(string gameKey) {
            return this._matches[gameKey];
        }
    }
}