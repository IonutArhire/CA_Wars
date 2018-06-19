using Persistence.Entities;
using System.Linq;

namespace Persistence.Repositories
{
    public class LifeLikeRepo: ILifeLikeRepo
    {
        private readonly IDatabaseContext _context;

        public LifeLikeRepo(IDatabaseContext context)
        {
            this._context = context;
        }

        public LifeLike GetByName(string name) {
            return this._context.LifeLikes.First(x => x.Name == name);
        }
    }
}