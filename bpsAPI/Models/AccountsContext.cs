using Microsoft.EntityFrameworkCore;

namespace bpsAPI.Models
{
    public class AccountsContext : DbContext
    {
        public AccountsContext(DbContextOptions<AccountsContext> options)
    : base(options)
        {
        }

        public DbSet<Accounts> AccountItems { get; set; }
        public DbSet<Payments> PaymentItems { get; set; }
    }
}
