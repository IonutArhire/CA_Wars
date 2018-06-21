using System.Collections.Generic;

namespace Services.Models
{
    public class MatchModel
    {
        public MatchModel(DimensionsModel dimensions, int maxGenerations, int nrPlayers, List<PlayerModel> players, List<int> playerNumbers, float[,] map, LifeLikeModel ruleSet) 
        {
            this.Dimensions = dimensions;
            this.MaxGenerations = maxGenerations;
            this.NrPlayers = nrPlayers;
            this.Players = players;
            this.PlayerNumbers = playerNumbers;
            this.Map = map;
            this.RuleSet = ruleSet;

            this.InitialConfigs = new List<float[,]>();
        }

        public DimensionsModel Dimensions { get; private set; }

        public int MaxGenerations { get; private set; }

        public int NrPlayers { get; private set; }

        public List<PlayerModel> Players { get; private set; }
        
        public List<float[,]> InitialConfigs { get; private set; }

        public List<int> PlayerNumbers { get; set; }

        public float[,] Map { get; private set; }

        public LifeLikeModel RuleSet { get; private set; }
    }
}