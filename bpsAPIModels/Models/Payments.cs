using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace bpsAPIModels.Models
{
    public class Payments
    {
        public long Id { get; set; }
        public long AcctNumber { get; set; }
        public string PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }

        [ForeignKey("AcctNumber")]
        public Accounts Accounts { get; set; }
    }
}
