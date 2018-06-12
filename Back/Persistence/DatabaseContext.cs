using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence
{
    public class DatabaseContext: DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<LifeLike> LifeLikes { get; set; }
    }
}