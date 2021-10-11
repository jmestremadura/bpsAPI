using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bpsAPI.Models
{
    public class PaymentDetails
    {
        public string account_number { get; set; }
        public string account_name { get; set; }
        public string account_status { get; set; }
        public string payment_amount { get; set; }
        public string payment_date { get; set; }
    }

    public class PaymentDetailsReturnModel
    {
        public List<PaymentDetails> details { get; set; }
    }
}
