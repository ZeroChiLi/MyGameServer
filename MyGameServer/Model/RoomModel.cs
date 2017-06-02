using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGameServer.Model
{
    public class RoomModel
    {
        public int Id;
        public Dictionary<MyClientPeer, AccountModel> clientAccountDict;

        public RoomModel(int id)
        {
            Id = id;
            clientAccountDict = new Dictionary<MyClientPeer, AccountModel>();
        }

        //是否包含
        public bool Contains(MyClientPeer client)
        {
            return clientAccountDict.ContainsKey(client);
        }

        //添加房间
        public void Add(MyClientPeer client, AccountModel model)
        {
            clientAccountDict.Add(client,model);
        }

        //删除房间
        public void Remove(MyClientPeer client)
        {
            clientAccountDict.Remove(client);
        }
    }
}
