using System;
using bpsAPIModels.Models;
using bpsAPISrv.APIContext;

namespace bpsAPISrv
{
    public class PaymentPersist
    {
        public ReturnValue SavePayment(Payments payments, dbAPIContext _dbAPIContext)
        {
            var retval = new ReturnValue();
            var objCommonMethods = new CommonMethods();
            var objAccountPersist = new AccountPersist();

            retval = objCommonMethods.ValidatePayment(payments, _dbAPIContext);

            if (retval.isValid)
            {

                payments.PaymentDate = DateTime.Now;
                _dbAPIContext.PaymentItems.Add(payments);
                _dbAPIContext.SaveChanges();

                //Account is for Closure
                if (retval.ERR_CODE == "100")
                {
                    retval = objAccountPersist.CloseAccount(payments, _dbAPIContext);
                }
            }


            return retval;
        }
    }
}
