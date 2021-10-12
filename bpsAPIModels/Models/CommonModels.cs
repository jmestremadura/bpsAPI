using System;
using System.Collections.Generic;
using System.Text;

namespace bpsAPIModels.Models
{
    public class ReturnValue
    {
        public bool isValid { get; set; }
        public string ERR_CODE { get; set; }
        public string ERR_MSG { get; set; }
       
    }
}
