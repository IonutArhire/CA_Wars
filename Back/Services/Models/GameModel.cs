using System.Collections.Generic;

namespace Services.Models
{
    public class GameModel
    {
        public GameModel(DimensionsModel dimensions, int nrPlayers, int maxGenerations, List<PlayerModel> players, List<int> playerNumbers, float[,] map) 
        {
            this.Dimensions = dimensions;
            this.NrPlayers = nrPlayers;
            this.MaxGenerations = maxGenerations;
            this.Players = players;
            this.PlayerNumbers = playerNumbers;
            this.Map = map;

            this.Configs = new List<float[,]>();
        }

        public DimensionsModel Dimensions { get; set; }

        public int NrPlayers { get; set; }

        public int MaxGenerations { get; set; }

        public List<PlayerModel> Players { get; set; }
        
        public List<float[,]> Configs { get; set; }

        public List<int> PlayerNumbers { get; set; }

        public float[,] Map { get; set; }
    }
}