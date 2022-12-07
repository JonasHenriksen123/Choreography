using Microsoft.EntityFrameworkCore;

namespace Accounts.Model
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<Event> Events { get; set; }
         
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {

        }
    }
}
