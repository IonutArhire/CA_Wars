using Services.Models;

namespace Services.RuleSetService
{
    public interface IRuleSetService
    {
        LifeLikeModel GetByName(string name);
    }
}