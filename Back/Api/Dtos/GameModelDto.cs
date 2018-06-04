using System.Collections.Generic;
using Services.Models;

namespace Api.Dtos
{
    public class GameModelDto
    {
        public GameModelDto(DimensionsModel dimensions, int assignedNumber, int nrPlayers, int maxGenerations, List<PlayerModel> players, float[,] map) 
        {
            this.Dimensions = dimensions;
            this.AssignedNumber = assignedNumber;
            this.NrPlayers = nrPlayers;
            this.MaxGenerations = maxGenerations;
            this.Players = players;
            this.Map = map;

            this.Configs = new List<float[,]>();
        }

        public DimensionsModel Dimensions { get; set; }

        public int AssignedNumber { get; set; }

        public int NrPlayers { get; set; }

        public int MaxGenerations { get; set; }

        public List<PlayerModel> Players { get; set; }
        
        public List<float[,]> Configs { get; set; }

        public float[,] Map { get; set; }
    }
}