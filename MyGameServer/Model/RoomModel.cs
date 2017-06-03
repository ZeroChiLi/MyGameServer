using System.Collections.Generic;

namespace MyGameServer.Model
{
    //房间模型
    public class RoomModel
    {
        public int Id;

        //连接的客户端和所对应的用户模型
        public Dictionary<MyClientPeer, AccountModel> clientAccountDict;

        public RoomModel(int id)
        {
            Id = id;
            clientAccountDict = new Dictionary<MyClientPeer, AccountModel>();
        }

        //是否包含客户端
        public bool Contains(MyClientPeer client)
        {
            return clientAccountDict.ContainsKey(client);
        }

        //添加客户端及其对应的用户到房间
        public void Add(MyClientPeer client, AccountModel model)
        {
            clientAccountDict.Add(client,model);
        }

        //把客户端从房间中移除
        public void Remove(MyClientPeer client)
        {
            clientAccountDict.Remove(client);
        }
    }
}
