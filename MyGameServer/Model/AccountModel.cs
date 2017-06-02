using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGameServer.Model
{
    public class AccountModel
    {
        public int Id;
        public string Account;
        public string Password;

        public AccountModel(int id, string account, string password)
        {
            Id = id;
            Account = account;
            Password = password;
        }
    }
}
