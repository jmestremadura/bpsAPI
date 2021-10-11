using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bpsAPI.Models
{
    public class Accounts
    {
        public long Id { get; set; }
        public string AcctNumber { get; set; }
        public string AcctName { get; set; }
        public string AcctBalance { get; set; }
        public string AcctStatus { get; set; }        
        public string AcctRemarks { get; set; }

    }


}
