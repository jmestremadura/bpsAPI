using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bpsAPI.Models
{
    public class Payments
    {
        public long Id { get; set; }
        public string AcctNumber { get; set; }
        public string PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
