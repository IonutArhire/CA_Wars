using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.AlgorithmService {
    public class AlgorithmService {

        private static float[][] _grid;

        private static int _size;

        private static int _nrPlayers = 2;

        private static float[][] CreateEmptyMatrix(int size) {
            return Enumerable.Range(0, size).Select(i => new float[size]).ToArray();
        }

        private static float[][] CopyMatrix(float[][] matrix) {
            var result = CreateEmptyMatrix(matrix.Length);

            for (var i = 0; i < matrix.Length; i++) {
                for (var j = 0; j < matrix.Length; j++) {
                    result[i][j] = matrix[i][j];
                }
            }

            return result;
        }

        private static float[][] CombineInitialConfigs(List<float[][]> configs, int size) {
            float[][] result = CreateEmptyMatrix(size);

            for (var i = 0; i < size; i++) {
                for (var j = 0; j < size; j++) {
                    for (var k = 0; k < configs.Count; k++) {
                        result[i][j] = configs[k][i][j];
                        if (configs[k][i][j] != -1) {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private static List<float> getNeighbors(int x, int z) {
            int mod(int l, int r) {
                return (l % r + r) % r;
            }

            var result = new List<float> ();

            for (int i = 0; i < _nrPlayers; i++) {
                result.Add(0);
            }

            for (int i = x - 1; i <= x + 1; i++) {
                for (int j = z - 1; j <= z + 1; j++) {
                    if (i == x && j == z) {
                        continue;
                    }

                    var iAux = mod(i, _size);
                    var jAux = mod(j, _size);

                    var owner = Convert.ToInt32 (_grid[iAux][jAux]);
                    if (owner != -1) {
                        result[owner] += 1;
                    }
                }
            }

            return result;
        }

        public static float[][] Initialize(List<float[][]> configs, int size) {
            _grid = CombineInitialConfigs(configs, size);
            _size = size;

            return _grid;
        }

        public static float[][] RunNextGen() {
            var GOF = new Dictionary<bool, List<float>> () { { false, new List<float> () { { 3 } } }, { true, new List<float> () { { 2 }, { 3 } } }
                };

            var resultGrid = CopyMatrix(_grid);

            for (var i = 0; i < _size; i++) {
                for (var j = 0; j < _size; j++) {
                    var neighbors = getNeighbors(i, j);
                    var candidatePlayers = new List<float>();
                    for (int playerIdx = 0; playerIdx < _nrPlayers; playerIdx++) {
                        var isAlive = _grid[i][j] == playerIdx ? true : false;
                        
                        if (GOF[isAlive].FindLast(x => x == neighbors[playerIdx]) != 0) {
                            candidatePlayers.Add(playerIdx);
                        }
                    }

                    if (candidatePlayers.Count == 1) {
                        resultGrid[i][j] = candidatePlayers[0];
                    } else {
                        resultGrid[i][j] = -1;
                    }
                }
            }

            _grid = CopyMatrix(resultGrid);
            return _grid;
        }

        public static bool isGridEmpty() {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_grid[i][j] != -1) {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}