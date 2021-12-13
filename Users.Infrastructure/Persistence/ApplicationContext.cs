using System.Data.Entity;
using Users.Core.Entities;

namespace Users.Infrastructure.Persistence
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}
