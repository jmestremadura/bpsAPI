using System.Linq;
using Microsoft.AspNetCore.Mvc;
using bpsAPIModels.Models;
using bpsAPISrv.APIContext;
using bpsAPISrv;

namespace bpsAPI.Controllers
{
    [Route("api/Accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly dbAPIContext _context;
        
        public AccountsController(dbAPIContext context)
        {
            _context = context;
        }

        //api/Accounts/GenerateAccount
        [HttpPost("GenerateAccount")]
        public ActionResult<ReturnValue> GenerateAccount(Accounts accounts)
        {
            var retval = new ReturnValue();
            var objAccountPersist = new AccountPersist();
            
            retval = objAccountPersist.SaveAccount(accounts, _context);

            return retval;
        }

        //api/Accounts/PostAccountPayment
        [HttpPost("PostAccountPayment")]
        public ActionResult<ReturnValue> PostPayment(Payments payments)
        {
            var retval = new ReturnValue();
            var objPaymentPersist = new PaymentPersist();

            retval = objPaymentPersist.SavePayment(payments, _context);

            return retval;
        }

        //api/Accounts/GetPaymentDetails
        [HttpGet("GetPaymentDetails/{AcctNumber}")]
        public IQueryable GenerateAccountPayments(long AcctNumber)
        {
            var objAccount = new AccountPersist();
            var result = objAccount.FetchAccountPayment(AcctNumber, _context);

            return result;
        }

    }
}
