using System;
using System.Collections.Generic;
using System.Linq;
using Services.Models;
using static Services.MatrixService.MatrixService;

namespace Services.AlgorithmService {
    public class AlgorithmService {

        private static float[,] _grid;

        private static DimensionsModel _dimensions;

        private static int _nrPlayers = 2;
        
        private static float[,] CombineInitialConfigs(List<float[,]> configs, DimensionsModel dimensions) {
            float[,] result = CreateEmptyMatrix(dimensions);

            for (var i = 0; i < dimensions.Height; i++) {
                for (var j = 0; j < dimensions.Width; j++) {
                    for (var k = 0; k < configs.Count; k++) {
                        var val = configs[k][i,j];
                        if (val == -2) {
                            val = -1;
                        }

                        result[i,j] = val;

                        if (val >= 0) {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private static List<float> createPlayerArray() {
            var result = new List<float>();

            for (int i = 0; i < _nrPlayers; i++) {
                result.Add(0);
            }

            return result;
        }

        private static List<float> getNeighbors(int x, int z) {
            int mod(int l, int r) {
                return (l % r + r) % r;
            }

            var owners = createPlayerArray();

            for (int i = x - 1; i <= x + 1; i++) {
                for (int j = z - 1; j <= z + 1; j++) {
                    if (i == x && j == z) {
                        continue;
                    }

                    var iAux = mod(i, _dimensions.Height);
                    var jAux = mod(j, _dimensions.Width);

                    var owner = Convert.ToInt32(_grid[iAux,jAux]);
                    if (owner >= 0) {
                        owners[owner] += 1;
                    }
                }
            }

            return owners;
        }

        public static float[,] Initialize(List<float[,]> configs, DimensionsModel dimensions) {
            _grid = CombineInitialConfigs(configs, dimensions);
            _dimensions = dimensions;

            return _grid;
        }

        public static float[,] RunNextGen() {
            var GOF = new Dictionary<bool, List<float>> () { { false, new List<float> () { { 3 } } }, { true, new List<float> () { { 2 }, { 3 } } }
                };

            var resultGrid = CopyMatrix(_grid);

            for (var i = 0; i < _dimensions.Height; i++) {
                for (var j = 0; j < _dimensions.Width; j++) {
                    var neighbors = getNeighbors(i, j);
                    var candidatePlayers = new List<float>();
                    for (int playerIdx = 0; playerIdx < _nrPlayers; playerIdx++) {
                        var isAlive = _grid[i,j] == playerIdx ? true : false;
                        
                        if (GOF[isAlive].FindLast(x => x == neighbors[playerIdx]) != 0) {
                            candidatePlayers.Add(playerIdx);
                        }
                    }

                    if (candidatePlayers.Count == 1) {
                        resultGrid[i,j] = candidatePlayers[0];
                    } else {
                        resultGrid[i,j] = -1;
                    }
                }
            }

            _grid = CopyMatrix(resultGrid);
            return _grid;
        }

        public static bool isGridEmpty() {
            for (int i = 0; i < _dimensions.Height; i++)
            {
                for (int j = 0; j < _dimensions.Width; j++)
                {
                    if (_grid[i,j] != -1) {
                        return false;
                    }
                }
            }

            return true;
        }

        public static int getWinner() {
            var owners = createPlayerArray();

            for (int i = 0; i < _dimensions.Height; i++)
            {
                for (int j = 0; j < _dimensions.Width; j++)
                {
                    var owner = Convert.ToInt32(_grid[i,j]);
                    if (owner != -1) {
                        owners[owner] += 1;
                    }
                }
            }

            var maxValue = owners.Max();
            var maxIndex = owners.IndexOf(maxValue);

            if (maxValue == 0) {
                return -1;
            } else {
                return maxIndex;
            }
        }
    }
}