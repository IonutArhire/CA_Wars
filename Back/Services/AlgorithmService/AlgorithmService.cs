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

        private int _nrPlayers;

        private LifeLikeModel _ruleSet;

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

        private float[,] Initialize(List<float[,]> configs, DimensionsModel dimensions, int nrPlayers, LifeLikeModel ruleSet) {
            _grid = CombineInitialConfigs(configs, dimensions);
            _dimensions = dimensions;

            this._nrPlayers = nrPlayers;
            this._ruleSet = ruleSet;

            return _grid;
        }

        private float[,] RunNextGen() {
            var resultGrid = this._matrixService.CopyMatrix(_grid);

            for (var i = 0; i < _dimensions.Height; i++) {
                for (var j = 0; j < _dimensions.Width; j++) {
                    var neighbors = getNeighbors(i, j);
                    var candidatePlayers = new List<float>();
                    for (int playerIdx = 0; playerIdx < _nrPlayers; playerIdx++) {
                        var isAlive = _grid[i,j] == playerIdx ? true : false;

                        if (isAlive) {
                            if (this._ruleSet.ForSurvival.Exists(x => x == neighbors[playerIdx])) {
                                candidatePlayers.Add(playerIdx);
                            }
                        }
                        else {
                            if (this._ruleSet.ForBirth.Exists(x => x == neighbors[playerIdx])) {
                                candidatePlayers.Add(playerIdx);
                            }
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

        private bool IsDraw(List<float> owners, float max) {
            var count = 0;

            for (int i = 0; i < owners.Count; i++)
            {
                if (owners[i] == max) {
                    count++;

                    if (count > 1) {
                        return true;
                    }
                }
            }

            return false;
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

            if (IsDraw(owners, maxValue)) {
                return -1;
            }
            else {
                var maxIndex = owners.IndexOf(maxValue);
                return maxIndex;
            }
        }

        public GameResultModel RunGame(MatchModel match) {
            var counter = 0;
                
            List<float[,]> generations = new List<float[,]>();
            generations.Add(this.Initialize(match.InitialConfigs, match.Dimensions, match.NrPlayers, match.RuleSet));
            while (!this.isGridEmpty() && counter != match.MaxGenerations) {
                generations.Add(this.RunNextGen());
                counter++;
            }

            return new GameResultModel(generations, this.getWinner());
        }
    }
}