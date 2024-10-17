using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public class BaseDbContext : DbContext
{
    public BaseDbContext()
    {
        
    }

    public virtual DbSet<Password> Passwords { get; set; }
    public virtual DbSet<User> Users { get; set; }
}
