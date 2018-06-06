using System;
using System.Collections.Generic;
using Services.MatrixService;
using Services.Models;

namespace Services.PlayerResourcesService
{
    public class PlayerResourcesService: IPlayerResourcesService
    {
        private IMatrixService _matrixService;

        public PlayerResourcesService(IMatrixService matrixService) {
            this._matrixService = matrixService;
        }
        
        private string GetNewColor(List<string> colors) {
            var randomizer = new Random();
            var idx = randomizer.Next(colors.Count);
            var result = colors[idx];
            colors.RemoveAt(idx);

            return result;
        }

        public List<string> InitializeColors() {
            return new List<string>()
            {   
                "#f28910", //orange
                "#099fef", //blue
                "#792ab2", //violet
                "#5bdb15"  //green
            };
        }

        public List<int> InitPlayerNumbers(int nrPlayers) {
            var result = new List<int>();
            for (int i = 0; i < nrPlayers; i++)
            {
                result.Add(i);
            }
            return result;
        }

        public int AssignNumber(GameModel game) {
            var playerNumbers = game.PlayerNumbers;

            var randomizer = new Random();
            var idx = randomizer.Next(playerNumbers.Count);
            var result = playerNumbers[idx];
            playerNumbers.RemoveAt(idx);

            return result;
        }
        
        public List<PlayerModel> GetPlayerResources(int nrPlayers) {
            var colors = InitializeColors();
            var results = new List<PlayerModel>();
            for (int i = 0; i < nrPlayers; i++)
            {
                var color = GetNewColor(colors);

                var playerResources = new PlayerModel(color, 0);

                results.Add(playerResources);
            }
            return results;
        }

        public float[,] GetPersonalizedMap(float[,] map, int assignedNumber) {
            var pmap = this._matrixService.CopyMatrix(map);

            for (int i = 0; i < pmap.GetLength(0); i++)
            {
                for (int j = 0; j < pmap.GetLength(1); j++)
                {
                    if(pmap[i,j] == assignedNumber) {
                        pmap[i,j] = -2;
                    }
                    else {
                        pmap[i,j] = -1;
                    }
                }
            }

            return pmap;
        }
    }
}