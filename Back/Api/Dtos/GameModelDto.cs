using System.Collections.Generic;
using Services.Models;

namespace Api.Dtos
{
    public class GameModelDto
    {
        public GameModelDto(DimensionsModel dimensions, int nrPlayers, int maxGenerations, List<PlayerModel> players, float[,] map) 
        {
            this.Dimensions = dimensions;
            this.NrPlayers = nrPlayers;
            this.MaxGenerations = maxGenerations;
            this.Players = players;
            this.Map = map;
        }

        public DimensionsModel Dimensions { get; set; }

        public int AssignedNumber { get; set; }

        public int NrPlayers { get; set; }

        public int MaxGenerations { get; set; }

        public List<PlayerModel> Players { get; set; }

        public float[,] Map { get; set; }
    }
}