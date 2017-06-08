
namespace MyGameServer.Model
{
    //用户模型
    public class AccountModel
    {
        public string AccountName;
        public string Password;

        public AccountModel(string accountName, string password)
        {
            AccountName = accountName;
            Password = password;
        }
    }
}
