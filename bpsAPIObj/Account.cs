using bpsAPIModels.Models;

namespace bpsAPIObj.AccountObj
{
    public class objAccount
    {

        public ReturnValue ValidateApplication(Accounts accounts)
        {
            var retval = new ReturnValue();

            //check if with existing accounts for generation using AcctNumber field
            //var existing_accounts = _context.AccountItems.Where(a => a.AcctNumber == accounts.AcctNumber).ToList();
            //if (existing_accounts.Count > 0)
            //{
            //    //[001] - With existing AcctNumber
            //    retval.ERR_CODE = "001";
            //    retval.ERR_MSG = "Account Number already exists, unable to Add.";
            //}



            return retval;
        }

        public ReturnValue GenerateApplication(Accounts accounts)
        {
            var retval = new ReturnValue();



            return retval;
        }

    }


}
