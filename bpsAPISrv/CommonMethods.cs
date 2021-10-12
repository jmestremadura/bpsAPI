using System;
using System.Linq;
using bpsAPIModels.Models;
using bpsAPISrv.APIContext;

namespace bpsAPISrv
{
    public class CommonMethods
    {

        public ReturnValue ValidateAccount(Accounts accounts, dbAPIContext _dbAPIContext)
        {
            var retval = new ReturnValue();
            
            //check if with existing accounts for generation using AcctNumber field
            var existing_accounts = _dbAPIContext.AccountItems.Where(a => a.AcctNumber == accounts.AcctNumber).ToList();
            if (existing_accounts.Count > 0)
            {
                //[001] - With existing AcctNumber
                retval.isValid = false;
                retval.ERR_CODE = "001";
                retval.ERR_MSG = "Account Number already exists, unable to Add.";
            }
            else
            {
                retval.isValid = true;
            }
            
            return retval;
        }

        public ReturnValue ValidatePayment (Payments payments, dbAPIContext _dbAPIContext)
        {
            var retval = new ReturnValue();
            decimal dAcctBalance = 0;
            decimal dCurrentPaymentAmount = 0;
            decimal dExistingPayment = 0;
            decimal dTotalPayment = 0;

            //check if account is existing before payment
            var existing_accounts = _dbAPIContext.AccountItems.FirstOrDefault(a => a.AcctNumber == payments.AcctNumber);

            //Validation if AcctNumber is existing
            if (existing_accounts == null)
            {
                //[002] - Account Number not existing
                retval.isValid = false;
                retval.ERR_CODE = "002";
                retval.ERR_MSG = "Invalid Account Number. Unable to Post Payment.";
                return retval;
            }

            //Validation if Account is still Active
            if (existing_accounts.AcctStatus == "C")
            {
                //[004] - Account Number already Closed.
                retval.isValid = false;
                retval.ERR_CODE = "004";
                retval.ERR_MSG = "Account already closed. Unable to Post Payment.";
                return retval;
            }

            //Validation if the payment exceeds the current balance
            dAcctBalance = existing_accounts.AcctBalance;
            dCurrentPaymentAmount = Int32.Parse(payments.PaymentAmount);

            if (dCurrentPaymentAmount > dAcctBalance)
            {
                //[003] - Payment exceeds the current balance
                retval.isValid = false;
                retval.ERR_CODE = "003";
                retval.ERR_MSG = "Invalid Payment. Payment exceeds the current balance for this Account.";
                return retval;
            }

            var existing_payments = _dbAPIContext.PaymentItems.Where(a => a.AcctNumber == payments.AcctNumber).ToList();
            dExistingPayment = existing_payments.Select(i => int.Parse(i.PaymentAmount)).Sum();
            dTotalPayment = dExistingPayment + dCurrentPaymentAmount;

            //Validation if the existing payments and current payment is within the current Balance of the Account
            if (dTotalPayment > dAcctBalance)
            {
                //[003] - Payment exceeds the current balance
                retval.isValid = false;
                retval.ERR_CODE = "003";
                retval.ERR_MSG = "Invalid Payment. Payment exceeds the current balance for this Account.";
                return retval;
            }

            //Validate if the Total Amount paid matches the Current Balance of the Account
            if (dTotalPayment == dAcctBalance)
            {
                //For Closure of Account
                retval.isValid = true;
                retval.ERR_CODE = "100"; 
                retval.ERR_MSG = "";
            } else
            {
                retval.isValid = true;
                retval.ERR_CODE = "000";
                retval.ERR_MSG = "";
            }

            return retval;
        }
    }


}

