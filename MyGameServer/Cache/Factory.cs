
namespace MyGameServer.Cache
{
    //用与获取用户列表
    public class Factory
    {
        public static AccountsCache accountCache = null;
        public static ChatCache chatCache = null;

        static Factory()
        {
            accountCache = new AccountsCache();
            chatCache = new ChatCache();
        }
    }
}
