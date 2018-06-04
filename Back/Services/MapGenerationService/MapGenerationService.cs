using System;
using Services.Models;
using static Services.MatrixService.MatrixService;

namespace Services.MapGenerationService
{
    public static class MapGenerationService
    {
        public static float[,] RandomGen(DimensionsModel dimensions, int nrPlayers) {
            var map = CreateEmptyMatrix(dimensions);
            var randomizer = new Random();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i,j] = randomizer.Next(nrPlayers);
                }
            }

            return map;
        }
    }
}