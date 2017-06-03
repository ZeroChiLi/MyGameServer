
namespace MyGameServer.Cache
{
    //用于获取用户和房间单例
    public class Factory
    {
        public static AccountsCache accountCache = null;
        public static RoomCache chatCache = null;

        static Factory()
        {
            accountCache = new AccountsCache();
            chatCache = new RoomCache();
        }
    }
}
