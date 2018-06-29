using Api.Resources;
using FluentValidation;
using Persistence.Repositories;

namespace Api.Validators
{
    public class MatchCreateResourceValidator: AbstractValidator<MatchCreateResource>
    {
        private ILifeLikeRepo _lifeLikeRepo;

        public MatchCreateResourceValidator(ILifeLikeRepo lifeLikeRepo) {
            this._lifeLikeRepo = lifeLikeRepo;

            RuleFor(mc => mc.NrPlayers).InclusiveBetween(1, 4);
            RuleFor(mc => mc.RuleSet).Must(BeAValidRuleSet);
            RuleFor(mc => mc.MaxIters).InclusiveBetween(1, 10000);
            RuleFor(mc => mc.Rows).InclusiveBetween(2, 50);
            RuleFor(mc => mc.Cols).InclusiveBetween(2, 50);
        }

        private bool BeAValidRuleSet(string ruleset) {
            try {
                this._lifeLikeRepo.GetByName(ruleset);
                return true;
            }
            catch {
                return false;
            }
        }
    }
}