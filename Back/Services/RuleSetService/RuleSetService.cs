using System;
using System.Collections.Generic;
using Persistence.Repositories;
using Services.Models;

namespace Services.RuleSetService
{
    public class RuleSetService: IRuleSetService
    {
        private ILifeLikeRepo _lifeLikeRepo;

        private List<byte> ParseNeighbours(string neighbours) {
            var result = new List<byte>();

            foreach (char c in neighbours) {
                result.Add((byte)Char.GetNumericValue(c));
            }

            return result;
        }

        public RuleSetService(ILifeLikeRepo lifeLikeRepo) {
            this._lifeLikeRepo = lifeLikeRepo;
        }

        public LifeLikeModel GetByName(string name) {
            var repoResult = this._lifeLikeRepo.GetByName(name);
                
            var forSurvival = this.ParseNeighbours(repoResult.ForSurvival);
            var forBirth = this.ParseNeighbours(repoResult.ForBirth);

            var newLifeLikeModel = new LifeLikeModel(repoResult.Name, forSurvival, forBirth, repoResult.Character);

            return newLifeLikeModel;
        }
    }
}