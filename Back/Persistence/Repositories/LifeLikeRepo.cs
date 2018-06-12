using Persistence.Entities;
using System.Linq;

namespace Persistence.Repositories
{
    public class LifeLikeRepo: ILifeLikeRepo
    {
        private readonly IDatabaseContext _context;

        public LifeLikeRepo(IDatabaseContext context)
        {
            _context = context;
        }

        public LifeLike GetByName(string name) {
            return _context.LifeLikes.First(x => x.Name == name);
        }
    }
}