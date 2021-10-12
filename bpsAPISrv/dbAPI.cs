using Microsoft.EntityFrameworkCore;
using bpsAPIModels.Models;

namespace bpsAPISrv.APIContext
{
    public class dbAPIContext : DbContext

    {
        public dbAPIContext(DbContextOptions<dbAPIContext> options)
: base(options)
        {
        }

        public DbSet<Accounts> AccountItems { get; set; }
        public DbSet<Payments> PaymentItems { get; set; }

    }
}
