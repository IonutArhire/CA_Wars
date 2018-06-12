using Persistence.Entities;

namespace Persistence.Repositories
{
    public interface ILifeLikeRepo
    {
        LifeLike GetByName(string name);
    }
}