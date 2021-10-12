﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace bpsAPI.Models
{
    public class Accounts
    {
        public long Id { get; set; }
        public long AcctNumber { get; set; }    
        public string AcctName { get; set; }
        public decimal AcctBalance { get; set; }
        public string AcctStatus { get; set; }
        public string AcctRemarks { get; set; }

        public virtual ICollection<Payments> PaymentList { get; set; }
    }


}
