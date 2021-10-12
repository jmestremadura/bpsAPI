# bpsAPI
sample payment / inquiry API

# Pre-requisites
1) ensure that the ff is imported 
for configuration of the application, ensure that the ff are installed on your packages are installed in your project:

   -Microsoft.EntityFrameworkCore
   
   -Microsoft.EntityFrameworkCore.InMemory

2) For testing purposes, you may use the POSTMAN App (https://www.postman.com/downloads/) *ensure that the SSL Certificate verification is disabled on the PostMan App.

# Application Parameters
--<h3>POST</h3>
<br>
<b><i>GENERATE ACCOUNT</i></b><br>
URL: <u>https://(ipaddress):(port)/api/Accounts/GenerateAccount</u> <br>

This service will insert an account in the sample payment API.<br>
Sample REQUEST JSON String: <br>
{
  "acctnumber":100,
  "acctname":"Juan Dela Cruz",
  "acctbalance":1500
}

Sample RESPONSE JSON String: <br>
{
    "isValid":true,
    "erR_CODE": "000",
    "erR_MSG": ""
}


<b><i>POST ACCOUNT PAYMENT</i></b><br>
URL: https://(ipaddress):(port)/api/Accounts/PostAccountPayment <br>

This service will post a payment on a speciic account number generated in GenerateAccount Service.<br>
Sample REQUEST JSON String: <br>

{
  "acctnumber":100,
  "paymentamount": "800"
}

Sample RESPONSE JSON String: <br>
{
    "isValid":true,
    "erR_CODE": "000",
    "erR_MSG": ""
}

--<h3>GET</h3>
<br>
<b><i>GET PAYMENT DETAILS</i></b><br>
This service will get all the payment details on a speciic account number.<br>
URL: <u>https://(ipaddress):(port)/api/Accounts/GetPaymentDetails/(acctnumber)</u> <br>

Sample URL:
https://localhost:44327/api/Accounts/GetPaymentDetails/001

Sample RESPONSE JSON String: <br>
[<br>
    { <br>
        "account_number": "001", <br>
        "account_name": "Juan Dela Cruz", <br>
        "account_status": "A", <br>
        "payment_amount": "800", <br>
        "payment_date": "2021-10-11T10:51:26.1089027+08:00" <br>
    }, <br>
    { <br>
        "account_number": "001", <br>
        "account_name": "Juan Dela Cruz", <br>
        "account_status": "A", <br>
        "payment_amount": "500", <br>
        "payment_date": "2021-10-11T10:51:21.6154055+08:00" <br>
    } <br>
] <br>

# Assumptions
1. Newly Generated Accounts will have a default Account Status of [A] = Active
2. Accounts with completed payment (acctbalance == total paymentamount) will automatically tag the Account as [C] = Closed

<b><i>Validation</i></b><br>
1. System will not allow creation of accounts with duplicate acctnumber.
2. System will not allow posting of payment to a non-existing acctnumber.
3. System will not allow posting of payments exceeding the current account balance.
4. System will not allow posting of payments if the account is already closed.

<b><i>Error Codes</i></b><br>
[000] - Succesfully posted. <br>
[001] - Account Number already exists, unable to Add. <br>
[002] - Invalid Account Number. Unable to Post Payment. <br>
[003] - Invalid Payment. Payment exceeds the current balance for this Account. <br>
[004] - Account already closed. Unable to Post Payment.

# Code Architecture <br>
<b>bpsAPI </b><br>
Project wherein the API post and request are found.<br>
The API primarily provides JSON string response if the transaction is successful as seen below:<br>
{
    "isValid":true,
    "erR_CODE": "000",
    "erR_MSG": ""
}


<b>bpsModels </b><br>
Project wherein models are defined to be used by the different projects. (bpsAPI and bpsAPISrv)

<b>bpsAPISrv </b><br>
Project wherein the business objects, validation and database (thru EF Core) interaction takes place.


# Test Scenarios
Scenario 1
Posting of Account:
![alt text](https://github.com/jmestremadura/bpsAPI/blob/master/bpsAPI/_images/1.jpg?raw=true)

Scenario 2
Posting of Account with the same account number:
![alt text](https://github.com/jmestremadura/bpsAPI/blob/master/bpsAPI/_images/2.jpg?raw=true)

Scenario 3
Posting of Payment to a specific account number:
![alt text](https://github.com/jmestremadura/bpsAPI/blob/master/bpsAPI/_images/3.jpg?raw=true)

Scenario 4
Posting of Payment exceeding the current balance:
![alt text](https://github.com/jmestremadura/bpsAPI/blob/master/bpsAPI/_images/4.jpg?raw=true)

Scenario 5
Posting of Payment to a non existing account number:
![alt text](https://github.com/jmestremadura/bpsAPI/blob/master/bpsAPI/_images/5.jpg?raw=true)

Scenario 6
Viewing of Payments made to an Account:
![alt text](https://github.com/jmestremadura/bpsAPI/blob/master/bpsAPI/_images/6.jpg?raw=true)

Scenario 7
Viewing of Payments if the total balance has been settled:
![alt text](https://github.com/jmestremadura/bpsAPI/blob/master/bpsAPI/_images/7.jpg?raw=true)
