using Microsoft.EntityFrameworkCore;

namespace bpsAPI.Models
{
    public class PaymentsContext : DbContext
    {
        public PaymentsContext(DbContextOptions<PaymentsContext> options)
: base(options)
        {
        }

        public DbSet<Payments> PaymentItems { get; set; }
    }
}
