using System.Collections.Generic;

namespace Services.Models
{
    public class GameModel
    {
        public GameModel(int size, int nrPlayers, int maxGenerations, List<PlayerModel> players, List<float[][]> configs) 
        {
            this.Size = size;
            this.NrPlayers = nrPlayers;
            this.MaxGenerations = maxGenerations;
            this.Players = players;
            this.Configs = configs;
        }

        public int Size { get; set; }

        public int NrPlayers { get; set; }

        public int MaxGenerations { get; set; }

        public List<PlayerModel> Players { get; set; }
        
        public List<float[][]> Configs { get; set; }
    }
}