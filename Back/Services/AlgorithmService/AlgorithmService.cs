using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.AlgorithmService {
    public class AlgorithmService {

        private static float[][] grid;

        private static int _size;

        private static int _nrPlayers = 2;

        private static float[][] CombineConfigs (List<float[][]> configs, int size) {
            float[][] result = Enumerable.Range (0, size).Select (i => new float[size]).ToArray ();
            for (var i = 0; i < size; i++) {
                for (var j = 0; j < size; j++) {
                    result[i][j] = -1;
                }
            }
            for (var i = 0; i < size; i++) {
                for (var j = 0; j < size; j++) {
                    for (var z = 0; z < configs.Count; z++) {
                        if (configs[z][i][j] != -1) {
                            result[i][j] = configs[z][i][j];
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public static float[][] Initialize (List<float[][]> configs, int size) {
            grid = CombineConfigs (configs, size);
            _size = size;

            return grid;
        }

        private static int mod (int x, int m) {
            return (x % m + m) % m;
        }

        private static List<float> getNeighbors (int x, int z) {
            var result = new List<float> ();

            for (int i = 0; i < _nrPlayers; i++) {
                result.Add (0);
            }

            for (int i = x - 1; i <= x + 1; i++) {
                for (int j = z - 1; j <= z + 1; j++) {
                    if (i == x && j == z) {
                        continue;
                    }

                    var iAux = mod(i, _size);
                    var jAux = mod(j, _size);

                    var owner = Convert.ToInt32 (grid[iAux][jAux]);
                    if (owner != -1) {
                        result[owner] += 1;
                    }
                }
            }

            return result;
        }

        private static float[][] CopyMatrix(float[][] matrix) {
            var result = Enumerable.Range (0, matrix.Length).Select (i => new float[matrix.Length]).ToArray ();

            for (var i = 0; i < matrix.Length; i++) {
                for (var j = 0; j < matrix.Length; j++) {
                    result[i][j] = matrix[i][j];
                }
            }

            return result;
        }

        public static float[][] RunNextGen () {
            var GOF = new Dictionary<bool, List<float>> () { { false, new List<float> () { { 3 } } }, { true, new List<float> () { { 2 }, { 3 } } }
                };

            var resultGrid = CopyMatrix(grid);

            for (var i = 0; i < _size; i++) {
                for (var j = 0; j < _size; j++) {
                    var neighbors = getNeighbors (i, j);
                    var results = new List<float> ();
                    for (int z = 0; z < _nrPlayers; z++) {
                        var isAlive = grid[i][j] == z ? true : false;
                        var result = Convert.ToInt32(GOF[isAlive].FindLast(x => x == neighbors[z]));
                        if (result != 0) {
                            results.Add (z);
                        }
                    }

                    if (results.Count == 1) {
                        resultGrid[i][j] = results[0];
                    } else {
                        resultGrid[i][j] = -1;
                    }
                }
            }

            grid = CopyMatrix(resultGrid);
            return grid;
        }

        public static bool isGridEmpty() {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (grid[i][j] != -1) {
                        return false;
                    }
                }
            }

            return true;
        }

    }
}