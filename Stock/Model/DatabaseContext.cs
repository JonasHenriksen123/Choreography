using Microsoft.EntityFrameworkCore;

namespace Stock.Model
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public DbSet<Event> Events { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    }
}
