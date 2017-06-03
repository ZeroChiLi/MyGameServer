
namespace MyGameServer.Model
{
    //用户模型
    public class AccountModel
    {
        public int Id;
        public string AccountName;
        public string Password;

        public AccountModel(int id, string accountName, string password)
        {
            Id = id;
            AccountName = accountName;
            Password = password;
        }
    }
}
