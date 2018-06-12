using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence
{
    public interface IDatabaseContext
    {
        DbSet<LifeLike> LifeLikes { get; set; }
    }
}