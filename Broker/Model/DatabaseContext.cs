using Microsoft.EntityFrameworkCore;

namespace Broker.Model
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {
        }
    }
}
