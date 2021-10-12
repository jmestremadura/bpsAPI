using bpsAPIModels.Models;
using bpsAPISrv.APIContext;
using System.Linq;

namespace bpsAPISrv
{

    public class AccountPersist
    {
        public IQueryable FetchAccountPayment(long AcctNumber, dbAPIContext _dbAPIContext)
        {
            var result = from TX_ACCOUNT in _dbAPIContext.AccountItems
                         join _TX_PAYMENT in _dbAPIContext.PaymentItems on TX_ACCOUNT.AcctNumber equals _TX_PAYMENT.AcctNumber into PAYMENTDETAILS
                         from TX_PAYMENT in PAYMENTDETAILS.DefaultIfEmpty()
                         where TX_PAYMENT.AcctNumber == AcctNumber
                         orderby TX_PAYMENT.PaymentDate descending
                         select new
                         {
                             account_number = TX_ACCOUNT.AcctNumber,
                             account_name = TX_ACCOUNT.AcctName,
                             account_status = TX_ACCOUNT.AcctStatus,
                             payment_amount = TX_PAYMENT.PaymentAmount,
                             payment_date = TX_PAYMENT.PaymentDate
                         };

            return result;
        }
        public ReturnValue SaveAccount(Accounts accounts, dbAPIContext _dbAPIContext)
        {
            var retval = new ReturnValue();
            var objCommonMethods = new CommonMethods();

            retval = objCommonMethods.ValidateAccount(accounts, _dbAPIContext);

            if (retval.isValid)
            {
                //Default AcctStatus to [A] = Active for newly generated accounts
                accounts.AcctStatus = "A";
                accounts.AcctRemarks = "";
                _dbAPIContext.AccountItems.Add(accounts);
                _dbAPIContext.SaveChanges();

                //[000] - Succesfully Saved
                retval.isValid = true;
                retval.ERR_CODE = "000";
                retval.ERR_MSG = "";
            }


            return retval;
        }

        public ReturnValue CloseAccount(Payments payments,dbAPIContext _dbAPIContext)
        {
            var retval = new ReturnValue();
            var objCommonMethods = new CommonMethods();
            var existing_accounts = _dbAPIContext.AccountItems.FirstOrDefault(a => a.AcctNumber == payments.AcctNumber);

            //[C] - Account Closed
            existing_accounts.AcctStatus = "C";
            existing_accounts.AcctRemarks = "[SYS] - Account Closed.";
            _dbAPIContext.SaveChanges();

            //[000] - Succesfully Saved
            retval.isValid = true;
            retval.ERR_CODE = "000";
            retval.ERR_MSG = "";

            return retval;
        }

    }

}
