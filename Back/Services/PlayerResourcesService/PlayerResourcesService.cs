using System;
using System.Collections.Generic;
using Services.Models;
using static Services.MatrixService.MatrixService;

namespace Services.PlayerResourcesService
{
    public class PlayerResourcesService
    {
        public static List<string> InitializeColors() {
            return new List<string>()
            {   
                "#f28910", //orange
                "#099fef", //blue
                "#792ab2", //violet
                "#5bdb15"  //green
            };
        }

        private static string GetNewColor(List<string> colors) {
            var randomizer = new Random();
            var idx = randomizer.Next(colors.Count);
            var result = colors[idx];
            colors.RemoveAt(idx);

            return result;
        }

        public static List<int> InitPlayerNumbers(int nrPlayers) {
            var result = new List<int>();
            for (int i = 0; i < nrPlayers; i++)
            {
                result.Add(i);
            }
            return result;
        }

        public static int AssignNumber(GameModel game) {
            var playerNumbers = game.PlayerNumbers;

            var randomizer = new Random();
            var idx = randomizer.Next(playerNumbers.Count);
            var result = playerNumbers[idx];
            playerNumbers.RemoveAt(idx);

            if (playerNumbers.Count == 0)
            {
                game.PlayerNumbers = InitPlayerNumbers(game.NrPlayers);
            }

            return result;
        }
        
        public static List<PlayerModel> GetPlayerResources(int nrPlayers) {
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

        public static float[,] GetPersonalizedMap(float[,] map, int assignedNumber) {
            var pmap = CopyMatrix(map);

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