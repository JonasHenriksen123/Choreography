using Microsoft.EntityFrameworkCore;

namespace Orders.Model
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderLine> OrderLines { get; set;}

        public DbSet<Event> Events { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    }
}
