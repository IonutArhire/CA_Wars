using System;
using System.Collections.Generic;
using System.Linq;
using Services.MatrixService;
using Services.Models;

namespace Services.AlgorithmService {
    public class AlgorithmService: IAlgorithmService
    {
        private float[,] _grid;

        private DimensionsModel _dimensions;

        private int _nrPlayers = 2;

        private IMatrixService _matrixService;

        public AlgorithmService(IMatrixService matrixService) {
            this._matrixService = matrixService;
        }
        
        private float[,] CombineInitialConfigs(List<float[,]> configs, DimensionsModel dimensions) {
            float[,] result = this._matrixService.CreateEmptyMatrix(dimensions);

            for (var i = 0; i < dimensions.Height; i++) {
                for (var j = 0; j < dimensions.Width; j++) {

                    result[i,j] = -1;

                    for (var k = 0; k < configs.Count; k++) {
                        var cellValue = configs[k][i,j];

                        if (cellValue >= 0) {
                            result[i,j] = cellValue;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private List<float> createPlayerArray() {
            var result = new List<float>();

            for (int i = 0; i < _nrPlayers; i++) {
                result.Add(0);
            }

            return result;
        }

        private List<float> getNeighbors(int x, int z) {
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

        private float[,] Initialize(List<float[,]> configs, DimensionsModel dimensions, int nrPlayers) {
            _grid = CombineInitialConfigs(configs, dimensions);
            _dimensions = dimensions;

            this._nrPlayers = nrPlayers;

            return _grid;
        }

        private float[,] RunNextGen() {
            var GOF = new Dictionary<bool, List<float>> () { { false, new List<float> () { { 3 } } }, { true, new List<float> () { { 2 }, { 3 } } }
                };

            var resultGrid = this._matrixService.CopyMatrix(_grid);

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

            _grid = this._matrixService.CopyMatrix(resultGrid);
            return _grid;
        }

        private bool isGridEmpty() {
            for (int i = 0; i < _dimensions.Height; i++)
            {
                for (int j = 0; j < _dimensions.Width; j++)
                {
                    if (_grid[i,j] >= 0) {
                        return false;
                    }
                }
            }

            return true;
        }

        private int getWinner() {
            var owners = createPlayerArray();

            for (int i = 0; i < _dimensions.Height; i++)
            {
                for (int j = 0; j < _dimensions.Width; j++)
                {
                    var owner = Convert.ToInt32(_grid[i,j]);
                    if (owner >= 0) {
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

        public GameResultModel RunGame(GameModel game) {
            var counter = 0;
                
            List<float[,]> generations = new List<float[,]>();
            generations.Add(this.Initialize(game.InitialConfigs, game.Dimensions, game.Players.Count));
            while (!this.isGridEmpty() && counter != game.MaxGenerations) {
                generations.Add(this.RunNextGen());
                counter++;
            }

            return new GameResultModel(generations, this.getWinner());
        }
    }
}