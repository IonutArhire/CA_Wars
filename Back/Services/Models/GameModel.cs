using System.Collections.Generic;

namespace Services.Models
{
    public class GameModel
    {
        public GameModel(DimensionsModel dimensions, int maxGenerations, List<PlayerModel> players, List<int> playerNumbers, float[,] map, LifeLikeModel ruleSet) 
        {
            this.Dimensions = dimensions;
            this.MaxGenerations = maxGenerations;
            this.Players = players;
            this.PlayerNumbers = playerNumbers;
            this.Map = map;
            this.RuleSet = ruleSet;

            this.InitialConfigs = new List<float[,]>();
        }

        public DimensionsModel Dimensions { get; private set; }

        public int MaxGenerations { get; private set; }

        public List<PlayerModel> Players { get; private set; }
        
        public List<float[,]> InitialConfigs { get; private set; }

        public List<int> PlayerNumbers { get; set; }

        public float[,] Map { get; private set; }

        public LifeLikeModel RuleSet { get; private set; }
    }
}