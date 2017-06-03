using MyGameServer.Model;

namespace MyGameServer.Cache
{
    //聊天缓存，其实房间模型缓存
    public class ChatCache
    {
        private RoomModel room;

        public ChatCache()
        {
            room = new RoomModel(0);
        }

        //客户端还有对应的用户模型加入房间
        public RoomModel Enter(MyClientPeer client, AccountModel model)
        {
            if (room.Contains(client))
                return null;

            room.Add(client, model);
            return room;
        }

        //客户端离开房间，返回对应的用户模型
        public AccountModel Leave(MyClientPeer client)
        {
            if (!room.clientAccountDict.ContainsKey(client))
                return null;

            AccountModel model = room.clientAccountDict[client];
            room.clientAccountDict.Remove(client);      //从房间模型中剔除这个客户端和其对应的用户模型
            return model;
        }

        //获取房间模型
        public RoomModel GetRoomModel()
        {
            return room;
        }
    }
}
