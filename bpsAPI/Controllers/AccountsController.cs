using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bpsAPI.Models;

namespace bpsAPI.Controllers
{
    [Route("api/Accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountsContext _context;

        public AccountsController(AccountsContext context)
        {
            _context = context;
        }

        //Creation of Account
        //api/Accounts/GenerateAccount
        [HttpPost("GenerateAccount")]
        public async Task<ActionResult<ReturnValue>> PostAccounts(Accounts accounts)
        {
            var retval = new ReturnValue();
            retval.ERR_CODE = "000";


            //check if with existing accounts for generation using AcctNumber field
            var existing_accounts = await _context.AccountItems.Where(a => a.AcctNumber == accounts.AcctNumber).ToListAsync();
            if (existing_accounts.Count > 0)
            {
                //[001] - With existing AcctNumber
                retval.ERR_CODE = "001";
                retval.ERR_MSG = "Account Number already exists, unable to Add.";
            }


            if (retval.ERR_CODE == "000")
            {
                //Default AcctStatus to [A] = Active for newly generated accounts
                accounts.AcctStatus = "A";
                accounts.AcctRemarks = "";

                _context.AccountItems.Add(accounts);
                await _context.SaveChangesAsync();

                //[000] - Succesfully Saved
                retval.ERR_CODE = "000";
                retval.ERR_MSG = "";
            }


            return retval;
        }

        //Creation of Payment
        //api/Accounts/PostAccountPayment
        [HttpPost("PostAccountPayment")]
        public async Task<ActionResult<ReturnValue>> PostPayment(Payments payments)
        {
            var retval = new ReturnValue();
            var iAcctBalance = 0;
            var iCurrentPaymentAmount = 0;
            var iExistingPayment = 0;
            var iTotalPayment = 0;
            var flgAcctClosed = false;

            retval.ERR_CODE = "000";

            //check if account is existing before payment
            var existing_accounts  = await _context.AccountItems.FirstOrDefaultAsync(a => a.AcctNumber == payments.AcctNumber);

            //----Validation of Payment Section START
            //Validation if AcctNumber is existing
            if (existing_accounts == null)
            {
                //[002] - Account Number not existing
                retval.ERR_CODE = "002";
                retval.ERR_MSG = "Invalid Account Number. Unable to Post Payment.";

                goto EndProcess;
            }

            //Validation if Account is still Active
            if (existing_accounts.AcctStatus == "C")
            {
                //[004] - Account Number already Closed.
                retval.ERR_CODE = "004";
                retval.ERR_MSG = "Account already closed. Unable to Post Payment.";

                goto EndProcess;
            }

            //Validation if the payment exceeds the current balance
            //iAcctBalance = Int32.Parse(existing_accounts[0].AcctBalance);
            iAcctBalance = Int32.Parse(existing_accounts.AcctBalance);
            iCurrentPaymentAmount = Int32.Parse(payments.PaymentAmount);

            if (iCurrentPaymentAmount > iAcctBalance)
            {
                //[003] - Payment exceeds the current balance
                retval.ERR_CODE = "003";
                retval.ERR_MSG = "Invalid Payment. Payment exceeds the current balance for this Account.";

                goto EndProcess;
            }

            var existing_payments = await _context.PaymentItems.Where(a => a.AcctNumber == payments.AcctNumber).ToListAsync();
            iExistingPayment = existing_payments.Select(i => int.Parse(i.PaymentAmount)).Sum();
            iTotalPayment = iExistingPayment + iCurrentPaymentAmount;

            //Validation if the existing payments and current payment is within the current Balance of the Account
            if (iTotalPayment > iAcctBalance)
            {
                //[003] - Payment exceeds the current balance
                retval.ERR_CODE = "003";
                retval.ERR_MSG = "Invalid Payment. Payment exceeds the current balance for this Account.";

                goto EndProcess;
            }

            //Validate if the Total Amount paid matches the Current Balance of the Account
            if (iTotalPayment == iAcctBalance)
            {
                flgAcctClosed = true;
            }
            //----Validation of Payment Section END


            //Saving of Payment Details
            if (retval.ERR_CODE == "000")
            {
                payments.PaymentDate = DateTime.Now;
                _context.PaymentItems.Add(payments);
                await _context.SaveChangesAsync();

                //[000] - Succesfully Saved
                retval.ERR_CODE = "000";
                retval.ERR_MSG = "";

                //Updating of Account Details.
                //The Account will automatically be closed once all of the balance has been succesfully settled.
                //Second Saving will be done for Account to ensure that the payment has been saved first
                if (flgAcctClosed) {
                    //[C] - Account Closed
                    existing_accounts.AcctStatus = "C";
                    existing_accounts.AcctRemarks = "[SYS] - Account Closed.";
                    await _context.SaveChangesAsync();
                }

            }

EndProcess:
            return retval;
        }

        //api/Accounts/GetPaymentDetails
        [HttpGet("GetPaymentDetails/{AcctNumber}")]
        public IQueryable GenerateAccountPayments(string AcctNumber)
        {

            var result = from TX_ACCOUNT in _context.AccountItems
                         join _TX_PAYMENT in _context.PaymentItems on TX_ACCOUNT.AcctNumber equals _TX_PAYMENT.AcctNumber into PAYMENTDETAILS
                         from TX_PAYMENT in PAYMENTDETAILS.DefaultIfEmpty() where TX_PAYMENT.AcctNumber == AcctNumber orderby TX_PAYMENT.PaymentDate descending 
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




        [HttpGet("FetchAllAccounts")]
        public async Task<ActionResult<IEnumerable<Accounts>>> FetchAllAccounts()
        {
            return await _context.AccountItems.ToListAsync();
        }

        [HttpGet("FetchAllPayments")]
        public async Task<ActionResult<IEnumerable<Payments>>> FetchAllPayments()
        {
            return await _context.PaymentItems.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Accounts>> GetAccounts(long id)
        {
            var accounts = await _context.AccountItems.FindAsync(id);

            if (accounts == null)
            {
                return NotFound();
            }

            return accounts;
        }

        public class ReturnValue
        {
            public string ERR_CODE { get; set; }
            public string ERR_MSG { get; set; }
        }


    }
}
