using System;
using System.Data;
using System.Data.SqlClient;

namespace Common.Dto
{
    [Serializable]
    public class AccountDto
    {
        public string Account;
        public string Password;

        //string connstring = "Data Source = (local);Initial Catalog = Accounts;Integrated Security = SSPI;";

        //public AccountDto(int Id)
        //{

        //}

    }
}
