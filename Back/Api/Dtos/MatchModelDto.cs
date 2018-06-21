using System.Collections.Generic;
using Services.Models;

namespace Api.Dtos
{
    public class MatchModelDto
    {
        public DimensionsModel Dimensions { get; set; }

        public int AssignedNumber { get; set; }

        public int MaxGenerations { get; set; }

        public List<PlayerModelDto> Players { get; set; }

        public float[,] Map { get; set; }
    }
}