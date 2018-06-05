using System;
using Services.MatrixService;
using Services.Models;
using static Services.MatrixService.MatrixService;

namespace Services.MapGenerationService
{
    public class MapGenerationService: IMapGenerationService
    {
        private IMatrixService _matrixService;

        public MapGenerationService(IMatrixService matrixService) {
            this._matrixService = matrixService;
        }

        public float[,] RandomGen(DimensionsModel dimensions, int nrPlayers) {
            var map = this._matrixService.CreateEmptyMatrix(dimensions);
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